using UnityEngine;

public class camera : MonoBehaviour
{
    private float X, Y, Z;
    public int speeds;
    private float eulerX = 0, eulerY = 0;
    [SerializeField] GameObject head;
    [SerializeField] GameObject body;
    // Use this for initialization
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        X = Input.GetAxis("Mouse X") * speeds * Time.deltaTime;
        Y = -Input.GetAxis("Mouse Y") * speeds * Time.deltaTime;
        eulerX = (head.transform.rotation.eulerAngles.x + Y) % 360;
        eulerY = (transform.rotation.eulerAngles.y + X) % 360;
        body.transform.rotation = Quaternion.Euler(0, eulerY, 0);
        head.transform.rotation = Quaternion.Euler(eulerX, body.transform.rotation.eulerAngles.y, 0);
    }
}
