using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VertexType
{
    LEG,LEGPART2,TORSO
}
public class Vertex : MonoBehaviour
{
    public float size;
    public Vector3 position;

    public Vector3 direction = Vector3.forward;
    public VertexType type;
    public Node node1;
    public Node node2;
    public GameObject physicobject;
    // Start is called before the first frame update
    void Start()
    {
        size = (node1.GetAnchor(this) - node2.GetAnchor(this)).magnitude;
        
    }

    public void FirstPlace() {
        position = node1.vertex1.position + node1.anchor1 - node1.anchor2;
        node1.transform.position = position + node1.anchor2;
        node2.transform.position = position + node2.anchor1;
        if(node2.vertex2 != null ) node2.vertex2.FirstPlace();
    }
 

    // Update is called once per frame
    public void UpdateAngle(Vector3 basedDirection)
    {
        Vector3 y = ((type == VertexType.TORSO) ? 1 : -1) * (node1.transform.position - node2.transform.position);
        Vector3 x = Vector3.Cross(basedDirection, y);
        Vector3 z = Vector3.Cross(y,x);
        
        direction = z.normalized;
        position = node1.transform.position + node1.GetAnchor(this).x * x + node1.GetAnchor(this).y * y + node1.GetAnchor(this).z * z;

        physicobject.transform.position = position;
        physicobject.transform.rotation = Quaternion.LookRotation(z, y);

        Debug.DrawLine(node1.transform.position, node2.transform.position, Color.blue,Time.deltaTime);
        Debug.DrawLine(this.position, this.position + direction.normalized * 0.2f, Color.red, Time.deltaTime);
        Debug.DrawLine(this.position, this.position + x.normalized * 0.2f, Color.green, Time.deltaTime);
    }
    public Node OtherNode(Node node)
    {
        if (node == node1) return node2;
        else if (node == node2) return node1;
        else return null;
    }
    public int GetPriority()
    {
        return (type == VertexType.TORSO)?2:(type == VertexType.LEG)?1:0;
    }
    public Vector3 GetYAxis()
    {
        Vector3 y = ((type == VertexType.TORSO) ? 1 : -1) * (node1.transform.position - node2.transform.position);
        return y;
    }
    public Vector3 GetXAxis()
    {
        Vector3 y = ((type == VertexType.TORSO) ? 1 : -1) * (node1.transform.position - node2.transform.position);
        Vector3 x = Vector3.Cross(direction, y);
        return x;
    }
}
