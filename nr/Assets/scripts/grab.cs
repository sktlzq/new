using System.Net.Http.Headers;
using UnityEngine;

public class grab : MonoBehaviour
{
    [SerializeField] GameObject me;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = new Ray();
            ray.origin = me.transform.position;
            ray.direction = me.transform.forward;
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (transform.gameObject.tag != "GND" && transform.gameObject.tag != "NotT")
                {
                    Destroy(hit.transform.gameObject);
                }
            }
        }
    }
}
