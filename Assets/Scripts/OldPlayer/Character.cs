using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Character : MonoBehaviour
{
    [SerializeField] private int SPEED = 3;
    private Rigidbody rigid;

    public bool leg = true;

    [SerializeField] private IKBone head;
    [SerializeField] private Limb leftarm;
    [SerializeField] private Limb rightarm;
    [SerializeField] private Limb leftleg;
    [SerializeField] private Limb rightleg;
    [SerializeField] private IKBone torso;

    [SerializeField] private GameObject leftlegdirection;
    [SerializeField] private GameObject rightlegdirection;
    [SerializeField] private GameObject leftarmdirection;
    [SerializeField] private GameObject rightarmdirection;


    // Start is called before the first frame update
    void Start()
    {
        this.rigid = GetComponent<Rigidbody>();
        head.transform.rotation = Quaternion.LookRotation(Vector3.forward);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));
        direction.Normalize();
        if(direction.magnitude < 0) head.transform.rotation = Quaternion.LookRotation(direction);

        if (leg/* && (rightleg.part2.SetDirection(false).AnchorRealPos() - rightlegdirection.transform.position).magnitude > 0.01f */)
        {
            //Debug.Log((rightleg.part2.SetDirection(false).AnchorRealPos() - rightlegdirection.transform.position).magnitude);
            Vector3 origin = leftlegdirection.transform.position;
            Vector3 objectif = rightlegdirection.transform.position;
            IKBone[] points = new IKBone[5];
            points[0] = leftleg.part2.SetDirection(true);
            points[1] = leftleg.SetDirection(true); 
            points[2] = torso.SetDirection(true);
            points[3] = rightleg.SetDirection(false);
            points[4] = rightleg.part2.SetDirection(false);
            FabrizioFull(points, origin, objectif);
        }
        else// if((leftleg.part2.SetDirection(false).AnchorRealPos() - leftlegdirection.transform.position).magnitude > 0.01f)
        {
            Vector3 origin = rightlegdirection.transform.position;
            Vector3 objectif = leftlegdirection.transform.position;
           
            IKBone[] points = new IKBone[5];
            points[0] = rightleg.part2.SetDirection(true);
            points[1] = rightleg.SetDirection(true);
            points[2] = torso.SetDirection(true);
            points[3] = leftleg.SetDirection(false);
            points[4] = leftleg.part2.SetDirection(false);

            FabrizioFull(points, origin, objectif);
        }
    }
    public void FabrizioFull(IKBone[] points, Vector3 origin, Vector3 objectif)
    {
        Vector3[] newpoints = new Vector3[points.Length];
        newpoints[points.Length - 1] = objectif;
        for(int i = points.Length-2; i >= 0; --i)
        {
            float size = points[i+1].size;

            newpoints[i] = newpoints[i + 1] + (points[i].AnchorRealPos() - newpoints[i + 1]).normalized * size;
            
            if (i < points.Length - 2 && points[i+1].GetAnchor().LimitRotate)
            {
                float angle = Vector3.Angle(newpoints[i + 2] - newpoints[i+1], newpoints[i] - newpoints[i+1]);
                angle = (angle + 360)%360;

                if (Clamp(angle,points[i+1].GetAnchor().MinRotate, points[i + 1].GetAnchor().MaxRotate) != angle) { 
                    Debug.Log(angle);
                }

            }
            
        }
        
        newpoints[0] = origin + (newpoints[0] - origin).normalized * points[0].size;
        for(int i = 1; i < points.Length; ++i)
        {
            float size = points[i].size;
            newpoints[i] = newpoints[i - 1] + (newpoints[i] - newpoints[i - 1]).normalized * size;

        }
        
        points[0].Replace(newpoints[0], origin);
        GetComponent<LineRenderer>().SetPositions(newpoints);

        for (int i = 1; i < points.Length; ++i)
        {
            points[i].Replace(newpoints[i], points[i - 1].AnchorRealPos());
            
        }
    }

    public void Ragdoll()
    {
        leftlegdirection.AddComponent<Rigidbody>();
        rightlegdirection.AddComponent<Rigidbody>();
    }
    public float Clamp(float angle, float min, float max)
    {
        if ((angle <= 180 && angle >= 180 - Mathf.Abs(min)) || (angle >= 180 && angle <= 180 + max))
        {
            return Mathf.Clamp(angle, 180 - Mathf.Abs(min), 180 + max);
        }

        if (angle > 180f)
        {
            angle -= 360f;
        }

        angle = Mathf.Clamp(angle, min, max);

        if (angle < 0f)
        {
            angle += 360f;
        }

        return angle;
    }
}
