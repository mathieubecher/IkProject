using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloup : MonoBehaviour
{
    [SerializeField] private GameObject p1;
    [SerializeField] private GameObject p2;
    [SerializeField] private GameObject p3;
    [SerializeField] private GameObject p4;
    [SerializeField] private GameObject p5;
    [SerializeField] private GameObject objectif;
    [SerializeField] private GameObject origin;
    public Vector3 anglebase;
    private float distance = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(true)
        {
            p1.transform.position = objectif.transform.position;
            p2.transform.position = p1.transform.position + (p2.transform.position - p1.transform.position).normalized * 2;
            p3.transform.position = p2.transform.position + (p3.transform.position - p2.transform.position).normalized * distance;
            p4.transform.position = p3.transform.position + (p4.transform.position - p3.transform.position).normalized * distance;
            p5.transform.position = p4.transform.position + (p5.transform.position - p4.transform.position).normalized * distance;
            // inverse
           
            p5.transform.position = origin.transform.position + (p5.transform.position - origin.transform.position).normalized * 2;
            p4.transform.position = p5.transform.position + (p4.transform.position - p5.transform.position).normalized * distance;
            p3.transform.position = p4.transform.position + (p3.transform.position - p4.transform.position).normalized * distance;
            p2.transform.position = p3.transform.position + (p2.transform.position - p3.transform.position).normalized * distance;
            p1.transform.position = p2.transform.position + (p1.transform.position - p2.transform.position).normalized * distance;
            
            p5.transform.rotation = Quaternion.LookRotation(origin.transform.position - p5.transform.position);
            p4.transform.rotation = Quaternion.LookRotation(p5.transform.position - p4.transform.position);
            p3.transform.rotation = Quaternion.LookRotation(p4.transform.position - p3.transform.position);
            p2.transform.rotation = Quaternion.LookRotation(p3.transform.position - p2.transform.position);
            p1.transform.rotation = Quaternion.LookRotation(p2.transform.position - p1.transform.position);
        }
    }
}
