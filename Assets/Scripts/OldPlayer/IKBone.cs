using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKBone : MonoBehaviour
{
    public Vector3 pos1;
    public Vector3 pos2;
    protected Vector3 objectpos1;
    protected Vector3 objectpos2;
    [SerializeField] protected Anchor anchor1;
    [SerializeField] protected Anchor anchor2;


    public float size;
    public bool direction = true;



    public void Start()
    {
        objectpos1 = this.transform.TransformPoint(pos1);
        objectpos2 = this.transform.TransformPoint(pos2);

        size = (objectpos1 - objectpos2).magnitude;
    }
    public virtual void Replace(Vector3 newpos, Vector3 previouspoint)
    {
        objectpos1 = (direction)?newpos: previouspoint;
        objectpos2 = (direction)?previouspoint:newpos;
        
        transform.position = objectpos2;
        transform.rotation = Quaternion.LookRotation(objectpos1 - objectpos2);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x + 90, transform.eulerAngles.y, transform.eulerAngles.z);

        transform.position = transform.TransformPoint((direction) ? pos1 : -pos2);
        

    }
    public Vector3 AnchorRealPos()
    {
        return (direction)?objectpos1:objectpos2;
        //return (direction) ? transform.TransformPoint(pos1) : transform.TransformPoint(pos2);
    }
    public IKBone SetDirection(bool direction = true)
    {
        this.direction = direction;
        return this;
    }

   
    public Anchor GetAnchor(bool pos = true)
    {
        return (direction) ? ((pos)?anchor1:anchor2) : ((pos) ? anchor2 : anchor1);
    }

}
