using UnityEngine;

public class Bitabit : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Activate()
    {
        this.gameObject.GetComponent<Rigidbody>().AddForce(this.transform.forward * 10000);
    }
}
