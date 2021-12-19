using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSegment : MonoBehaviour
{


    public Transform previousSegment;
    // Update is called once per frame



    private void Awake()
    {
        StartCoroutine(GetPreviousSegment());
    }
    void Update()
    {
        if (previousSegment != null)
        {
            transform.LookAt(previousSegment.position);
            //transform.forward = (previousSegment.position - transform.position).normalized;
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, previousSegment.rotation, 10 * Time.deltaTime * 50);
        }
    }
   
    IEnumerator GetPreviousSegment()
    {
        yield return new WaitForSeconds(0.1f);
        Transform parent = transform.parent;
        if(parent == null)
        {
            
        }
            
        for (int i = 1; i < parent.childCount; i++)
        {
            if (parent.GetChild(i).transform == transform)
            {
                              
                    previousSegment = parent.GetChild(i - 1).transform;
                

            }
        }
        yield return null;
    }
}
