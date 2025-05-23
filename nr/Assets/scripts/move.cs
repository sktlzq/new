using UnityEngine;

//��� ������� ����������� ��� ��� ������ �� ��������� 
//���� �� ������ ����� ������������� ��������� Rigidbody
[RequireComponent(typeof(Rigidbody))]
public class Move : MonoBehaviour
{
    public float Speed = 10f;
    public float JumpForce = 300f;
    [SerializeField] GameObject me;
    //��� �� ��� ���������� �������� �������� ��� "Ground" �� ���� ����������� �����
    private bool isGrounded;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // �������� �������� ��� ��� �������� � ������� 
    // ���������� ������������ � FixedUpdate, � �� � Update
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

                // �������� �������� ��� � ����� �� ������ Vector3.up 
                // � �� �� ������ transform.up. ���� �������� ���� ��� 
                // ���� �������� -- ���, �� ��� ������ "����" ����� 
                // ����� �����������. �����, ������, ����...
                // �� ��� ����� ������ ������ � ���������� �����, 
                // ������ � Vector3.up
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