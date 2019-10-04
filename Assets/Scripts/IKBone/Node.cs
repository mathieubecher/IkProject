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
    public float angleValue;
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
    public void AdaptAngleForward(Vertex objectif)
    {
        Node point3 = this;
        Vertex vertex2 = objectif;
        Node point2 = objectif.OtherNode(this);
        Vertex vertex1 = point2.OtherVertex(vertex2);
        Node point1 = vertex1.OtherNode(point2);

        Vertex parentAxis = (vertex1.GetPriority() > vertex2.GetPriority()) ? vertex1 : vertex2;
        Vertex childAxis = (vertex1.GetPriority() > vertex2.GetPriority()) ? vertex2 : vertex1;

        Vector3 axis = parentAxis.GetXAxis();

        if (Vector3.Angle((point1.transform.position - point2.transform.position), (point3.transform.position - point2.transform.position)) < 170)
        {
            axis = Vector3.Cross((point1.transform.position - point2.transform.position), (point3.transform.position - point2.transform.position)).normalized;
            axis = (Vector3.Angle(axis, parentAxis.GetXAxis()) > 90) ? -axis : axis;
        }


        point2.angleValue = (Vector3.SignedAngle((((parentAxis == vertex1) ? point3 : point1).transform.position - point2.transform.position),
            (((parentAxis == vertex1) ? point1 : point3).transform.position - point2.transform.position), axis) + 360) % 360;

        Debug.DrawLine(point2.transform.position, point2.transform.position + axis, Color.cyan);
        Debug.Log(point2.angleValue);

        if (point2.angleValue < point2.minAngle)
        {

            point2.angleValue = point2.minAngle;
            // Calculate angle min
            Vector3 minangle = new Vector3(0, Mathf.Cos(point2.angleValue * Mathf.PI / 180), Mathf.Sin(point2.angleValue * Mathf.PI / 180));
            minangle = axis * minangle.x + vertex1.GetYAxis() * minangle.y - Vector3.Cross(vertex1.GetYAxis(), axis) * minangle.z;
            minangle = (vertex1 == parentAxis) ? -minangle : minangle;
            minangle = minangle.normalized * vertex1.size;
            Debug.DrawLine(point2.transform.position, point2.transform.position + minangle, Color.magenta, Time.deltaTime);
            point3.transform.position = point2.transform.position + minangle;
        }
        else if (point2.angleValue > point2.maxAngle)
        {
            point2.angleValue = point2.maxAngle;
            // Calculate angle max
            Vector3 maxangle = new Vector3(0, Mathf.Cos(point2.angleValue * Mathf.PI / 180), Mathf.Sin(point2.angleValue * Mathf.PI / 180));
            maxangle = axis * maxangle.x + vertex1.GetYAxis() * maxangle.y - Vector3.Cross(vertex1.GetYAxis(), axis) * maxangle.z;
            maxangle = (vertex1 == parentAxis) ? -maxangle : maxangle;
            maxangle = maxangle.normalized * vertex1.size;
            Debug.DrawLine(point2.transform.position, point2.transform.position + maxangle, Color.magenta, Time.deltaTime);
            point3.transform.position = point2.transform.position + maxangle;
        }

    }

    public void Fabrik(Vertex objectif, Transform origin)
    {

        //forward
        transform.position = objectif.OtherNode(this).transform.position + (transform.position - objectif.OtherNode(this).transform.position).normalized * objectif.size;

        //Adapt Angle
        if (objectif.OtherNode(this).angle)
        {
            AdaptAngleForward(objectif);
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
