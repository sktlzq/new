using UnityEngine;

//��� ������� ���������� ��� ��� ������ �� ��������� ���� �� ������ ����� ������������� ������ ����������
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class Movement : MonoBehaviour
{
    public float Speed = 0.3f;
    public float JumpForce = 1f;

    //���� ����������� ������� ��� ����.
    //��� �� ��������� ��� ��� Player ��� �� ��������� � ������ ����. 

    //!!!!�������� �� ���� ������������� Layer, �������� Player!!!!
    public LayerMask GroundLayer = 3; 
    [SerializeField] GameObject me;
    private Rigidbody rb;
    private CapsuleCollider _collider; // ������ ��������� ������������ CapsuleCollider
    //� ������� ���� ��������� ���� �� ����

    private bool _isGrounded()
    {

        var bottomCenterPoint = new Vector3(_collider.bounds.center.x, _collider.bounds.min.y, _collider.bounds.center.z);
        //������� ��������� ���������� ������� � ��������� �� ���������� �� ��� ������ ������� ��������� � ����
        //_collider.bounds.size.x / 2 * 0.9f -- ��� �������� ����������� ����� ������ �������.  
        // ��� �� ����������� ������ -- ������ �� ������ ��������, � ��� ����� ��-�������������  
        return Physics.CheckCapsule(_collider.bounds.center, bottomCenterPoint, _collider.bounds.size.x / 2 * 0.9f, GroundLayer);  
        // ���� ����� ����� ������� � �������, �� ����� ����� �������� ���������� 0.9 �� �������.

    }


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _collider = GetComponent<CapsuleCollider>();

        //�.�. ��� �� ����� ��� �� �������� ��� ������ ��� ��-���� ��� ������ �� �� ��������.
        //�� ����� ��������� ������� �� ���� X � Z
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;

        //  ������ �� ������
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