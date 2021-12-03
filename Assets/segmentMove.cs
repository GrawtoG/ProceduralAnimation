using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class segmentMove : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector2 normalVec;
    public LegMove legMoveScript1;
    public LegMove legMoveScript2;
    Transform targetTransform1;
    Transform targetTransform2;
    Vector3 side1;
    void Awake()
    {
    
       
            if (legMoveScript1.targetTransform != null || legMoveScript2 != null)
            {
            targetTransform1 = legMoveScript1.targetTransform;
            targetTransform2 = legMoveScript2.targetTransform;
            }
          
        
            
    }


    // Update is called once per frame
    void Update()
    {
        RotatePerToNormalVectorLegs();
    }
    void RotatePerToNormalVectorLegs()
    {



        side1 = (targetTransform1.position - targetTransform2.position).normalized;
        
    

        normalVec = -Vector3.Cross(side1, transform.forward).normalized;


        transform.rotation = Quaternion.FromToRotation(transform.up, normalVec) * transform.rotation;
    }
}
