using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    public Vertex torso;
    public GameObject rightLegDirection;
    public GameObject leftLegDirection;
    public bool leg = true;
    // Start is called before the first frame update
    void Start()
    {
        torso.transform.position = Vector3.zero;
        torso.node1.vertex2.FirstPlace();
        leftLegDirection.transform.position = torso.node1.vertex2.node2.vertex2.node2.transform.position;
        torso.node2.vertex2.FirstPlace();
        rightLegDirection.transform.position = torso.node2.vertex2.node2.vertex2.node2.transform.position;

        

    }

    // Update is called once per frame
    void Update()
    {
        // objectif à droite :
        if(leg)
        {
            int i = 0;
            while (i < 10)
            {
                torso.node2.DeepSearch(torso).BeginFabrik(leftLegDirection, rightLegDirection);
                ++i;
            }
        }     
        else
            torso.node1.DeepSearch(torso).BeginFabrik(rightLegDirection, leftLegDirection);
    }
}
