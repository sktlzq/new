using UnityEngine;

public class Bitabit : MonoBehaviour
{
    float effect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        effect = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Activate()
    {
        this.gameObject.GetComponent<Rigidbody>().AddForce(this.transform.forward * 1000 * effect);
    }
    void EU()
    {
        effect *= 1.2f;
    }
    void ED()
    {
        effect /= 1.2f;
    }
}
