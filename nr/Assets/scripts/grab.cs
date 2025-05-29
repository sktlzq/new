using System.Net.Http.Headers;
using UnityEditor;
using UnityEngine;

public class grab : MonoBehaviour
{
    [SerializeField] GameObject me;
    private GameObject target;
    [SerializeField] Transform handpos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = null;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            if (target == null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 5))
                {
                    if (hit.transform.gameObject.tag != "GND" && hit.transform.gameObject.tag != "NotT")
                    {
                        target = hit.transform.gameObject;
                    }
                }
            }  
        }
        else
        {
            target = null;
        }


        if (target != null)
        {
            target.transform.rotation = handpos.rotation;
            target.transform.Find("grabpos").LookAt(handpos.position);
            target.GetComponent<Rigidbody>().AddForce(target.transform.Find("grabpos").forward * (Vector3.Distance(handpos.position, target.transform.position)) * 10, ForceMode.Impulse);
            target.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            if (Input.GetKey(KeyCode.E) && target.GetComponent<MonoBehaviour>())
            {
                MonoBehaviour script = target.GetComponent<MonoBehaviour>();
                if (script != null)
                {
                    script.Invoke("Activate", 0);
                }
            }
        }
        
    }
}

