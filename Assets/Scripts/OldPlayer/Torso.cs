using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torso : IKBone
{

    public override void Replace(Vector3 newpos, Vector3 previouspoint)
    {
        objectpos1 = (direction) ? newpos : previouspoint;
        objectpos2 = (direction) ? previouspoint : newpos;

        transform.position = objectpos2;
        transform.rotation = Quaternion.LookRotation(objectpos2 - objectpos1);
        transform.Rotate(Vector3.up, -90);

        transform.position = transform.TransformPoint((direction) ? -pos1 : pos2);
    }
}
