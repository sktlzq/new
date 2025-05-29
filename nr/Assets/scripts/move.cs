using UnityEngine;

//эти строчки гарантирют что наш скрипт не завалится если на плеере будет отсутствовать нужные компоненты
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class Movement : MonoBehaviour
{
    public float Speed = 0.3f;
    public float JumpForce = 1f;

    //даем возможность выбрать тэг пола.
    //так же убедитесь что ваш Player сам не относится к даному слою. 

    //!!!!Нацепите на него нестандартный Layer, например Player!!!!
    public LayerMask GroundLayer = 3; 
    [SerializeField] GameObject me;
    private Rigidbody rb;
    private CapsuleCollider _collider; // теперь прийдется использовать CapsuleCollider
    //и удалите бокс коллайдер если он есть

    private bool _isGrounded()
    {

        var bottomCenterPoint = new Vector3(_collider.bounds.center.x, _collider.bounds.min.y, _collider.bounds.center.z);
        //создаем невидимую физическую капсулу и проверяем не пересекает ли она обьект который относится к полу
        //_collider.bounds.size.x / 2 * 0.9f -- эта странная конструкция берет радиус обьекта.  
        // был бы обязательно сферой -- брался бы радиус напрямую, а так пишем по-универсальнее  
        return Physics.CheckCapsule(_collider.bounds.center, bottomCenterPoint, _collider.bounds.size.x / 2 * 0.9f, GroundLayer);  
        // если можно будет прыгать в воздухе, то нужно будет изменить коэфициент 0.9 на меньший.

    }


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _collider = GetComponent<CapsuleCollider>();

        //т.к. нам не нужно что бы персонаж мог падать сам по-себе без нашего на то указания.
        //то нужно заблочить поворот по осях X и Z
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;

        //  Защита от дурака
        if (GroundLayer == gameObject.layer)
            Debug.LogError("Player SortingLayer must be different from Ground SourtingLayer!");
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(me.transform.forward * Speed * 20);
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(-me.transform.forward * Speed * 20);
            
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(-me.transform.right * Speed * 20);            
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(me.transform.right * Speed * 20);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            JumpLogic();
        }
        rb.linearVelocity = Vector3.zero;

    }

    private void JumpLogic()
    {
        if (_isGrounded() && (Input.GetAxis("Jump") > 0))
        {
            rb.AddForce(Vector3.up * JumpForce / 10, ForceMode.Impulse);
        }
    }
}