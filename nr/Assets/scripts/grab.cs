using System.Net.Http.Headers;
using UnityEngine;

public class grab : MonoBehaviour
{
    [SerializeField] GameObject me;
    private GameObject target;
    [SerializeField] Transform grabpos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = null;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0) && target == null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.transform.gameObject.tag != "GND" && hit.transform.gameObject.tag != "NotT")
                {                        
                    target = hit.transform.gameObject;  
                }
            }
        }
        else if (Input.GetMouseButtonUp(0) && target != null)
        {
            target = null;
        }

        if (target != null)
        {
            target.transform.LookAt(grabpos.position);
            target.GetComponent<Rigidbody>().AddForce(target.transform.forward * 20, ForceMode.Impulse);
        }

    }
}
//Vector3.Distance(target.transform.position, grabpos.transform.position)
