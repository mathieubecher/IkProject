using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Vector3 anchor1;
    public Vector3 anchor2;
    public Vertex vertex1;
    public Vertex vertex2;
   
    public float minAngle = 0;
    public float maxAngle = 360;
    public bool angle = false;
    // Start is called before the first frame update
    void Start()
    {

        //transform.position = vertex1.transform.TransformPoint(anchor1);
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    public Vector3 GetAnchor(Vertex vertex)
    {
        if (vertex == vertex1) return anchor1;
        else if (vertex == vertex2) return anchor2;
        else return Vector3.zero;
    }

    public void BeginFabrik(GameObject origin, GameObject objectif)
    {
        transform.position = objectif.transform.position;
        Vertex next = ((vertex1 == null) ? vertex2 : vertex1);
        next.OtherNode(this).Fabrik(next, origin.transform);
        transform.position = next.OtherNode(this).transform.position + (transform.position - next.OtherNode(this).transform.position).normalized * next.size;

    }
    public void AdaptAngle(Vertex objectif, Transform origin, bool forward = true)
    {

        Node point1 = this;
        Vertex vertex1 = objectif;
        Node point2 = objectif.OtherNode(this);
        Vertex vertex2 = point2.OtherVertex(vertex1);
        Node point3 = vertex2.OtherNode(point2);

        Vertex axis = (vertex1.GetPriority() > vertex2.GetPriority()) ? vertex1 : vertex2;
        Vertex axisPos = (vertex1.GetPriority() > vertex2.GetPriority()) ? vertex2 : vertex1;

        Vector3 axisdirection = Vector3.Cross(point1.transform.position - point2.transform.position, point3.transform.position - point2.transform.position).normalized;
        axisdirection = (Vector3.Angle(axisdirection, axis.GetXAxis()) > 90) ? axisdirection : -axisdirection;
        //axisdirection = axis.GetXAxis();
        Debug.DrawLine(point2.transform.position, point2.transform.position + axisdirection, Color.cyan, Time.deltaTime);
        float angle = (Vector3.SignedAngle(point1.transform.position - point2.transform.position, point3.transform.position - point2.transform.position, axisdirection) + 360) % 360;
        if (vertex1.GetPriority() <= vertex2.GetPriority()) angle = -angle + 360;
            if (angle < point2.minAngle)

        {
            Vector3 minangle = new Vector3(0, Mathf.Cos(point2.minAngle * Mathf.PI / 180), Mathf.Sin(point2.minAngle * Mathf.PI / 180));
            minangle = axisPos.GetXAxis() * minangle.x + axisPos.GetYAxis() * minangle.y - axisPos.direction * minangle.z;
            transform.position = point2.transform.position + minangle;
            axisPos.UpdateAngle(axis.direction);
            Debug.DrawLine(point2.transform.position, point2.transform.position + minangle, Color.magenta,Time.deltaTime);

        }
        else if (angle > point2.maxAngle)
        {
            Vector3 maxangle = new Vector3(0, Mathf.Cos(point2.maxAngle * Mathf.PI / 180), Mathf.Sin(point2.maxAngle * Mathf.PI / 180));
            maxangle = axisPos.GetXAxis() * maxangle.x + axisPos.GetYAxis() * maxangle.y - axisPos.direction * maxangle.z;
            transform.position = point2.transform.position + maxangle;
            axisPos.UpdateAngle(axis.direction);
            Debug.DrawLine(point2.transform.position, point2.transform.position + maxangle, Color.yellow, Time.deltaTime);
        }
        
    }

    public void Fabrik(Vertex objectif, Transform origin)
    {

        //forward
        transform.position = objectif.OtherNode(this).transform.position + (transform.position - objectif.OtherNode(this).transform.position).normalized * objectif.size;

        //Adapt Angle
        if (objectif.OtherNode(this).angle)
        {
            AdaptAngle(objectif, origin);
        }

        //othervertex.Fabrik();
        Vertex next = (objectif == vertex1) ? vertex2 : vertex1;
        if (next != null)
        {
            next.OtherNode(this).Fabrik(next, origin);

            //backward
            transform.position = next.OtherNode(this).transform.position + (transform.position - next.OtherNode(this).transform.position).normalized * next.size;
        }
        else transform.position = origin.position;

        //Adapt Angle
        if (OtherVertex(objectif) != null && OtherVertex(objectif).OtherNode(this).angle)
        {
            AdaptAngle(OtherVertex(objectif), origin);
        }


        objectif.UpdateAngle(objectif.direction);

        
        

    }
    public Node DeepSearch(Vertex vertex)
    {
        if ((vertex == vertex1) ? vertex2 : vertex1 != null)
            return ((vertex == vertex1) ? vertex2 : vertex1).OtherNode(this).DeepSearch((vertex == vertex1) ? vertex2 : vertex1);
        else return this;
    }
    public Vertex OtherVertex(Vertex vertex)
    {
        if (vertex1 == vertex) return vertex2;
        else if (vertex2 == vertex) return vertex1;
        else return null;
    }
}
