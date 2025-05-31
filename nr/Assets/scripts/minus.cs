using System;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Minus : MonoBehaviour
{
    [SerializeField] GameObject minus;
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
        Ray ray = new Ray(minus.transform.position, minus.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            hit.transform.gameObject.transform.localScale /= (1.2f * effect);
            for (int i = 0; i < effect; i++)
            {
                hit.transform.gameObject.GetComponent<MonoBehaviour>().Invoke("ED", 0);
            }

        }
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
