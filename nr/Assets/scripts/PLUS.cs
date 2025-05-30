using System;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PLUS : MonoBehaviour
{
    [SerializeField] GameObject plus;
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
        Ray ray = new Ray(plus.transform.position, plus.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            hit.transform.gameObject.transform.localScale *= (1.2f * effect);
            for (int i = 0; i < effect; i++)
            {
                hit.transform.gameObject.GetComponent<MonoBehaviour>().Invoke("EU", 0);
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
