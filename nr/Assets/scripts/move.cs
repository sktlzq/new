using UnityEngine;

//эта строчка гарантирует что наш скрипт не завалитс€ 
//ести на плеере будет отсутствовать компонент Rigidbody
[RequireComponent(typeof(Rigidbody))]
public class Move : MonoBehaviour
{
    public float Speed = 10f;
    public float JumpForce = 300f;
    [SerializeField] GameObject me;
    //что бы эта переменна€ работала добавьте тэг "Ground" на вашу поверхность земли
    private bool isGrounded;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // обратите внимание что все действи€ с физикой 
    // необходимо обрабатывать в FixedUpdate, а не в Update
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
             rb.AddForce(me.transform.forward * Speed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(-me.transform.forward * Speed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(-me.transform.right * Speed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(me.transform.right * Speed);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            JumpLogic();
        }
        
    }






    private void JumpLogic()
    {
        if (Input.GetAxis("Jump") > 0)
        {
            if (isGrounded)
            {
                rb.AddForce(Vector3.up * JumpForce);

                // ќбратите внимание что € делаю на основе Vector3.up 
                // а не на основе transform.up. ≈сли персонаж упал или 
                // если персонаж -- шар, то его личный "верх" может 
                // любое направление. ¬лево, вправо, вниз...
                // Ќо нам нужен скачек только в абсолютный вверх, 
                // потому и Vector3.up
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        IsGroundedUpdate(collision, true);        
    }

    void OnCollisionExit(Collision collision)
    {
        IsGroundedUpdate(collision, false);
    }

    private void IsGroundedUpdate(Collision collision, bool value)
    {
        if (collision.gameObject.tag == ("GND"))
        {
            isGrounded = value;
        }
    }
}