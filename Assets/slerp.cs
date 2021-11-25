using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slerp : MonoBehaviour
{
    public Transform startPos;
    public Transform endPos;
    public float speed;
    public float journeyTime = 1.0f;
    public bool repeatable;


    private float startTime;
    Vector3 centerPoint;
    Vector3 startRelCenter;
    Vector3 endRelCenter;

    // Start is called before the first frame update
   

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(Move());
        }
        
      
        
        
    }
    void GetCenter(Vector3 direction)
    {
        centerPoint = (startPos.position + endPos.position) * 0.5f;
        centerPoint -= direction;
        startRelCenter = startPos.position - centerPoint;
        endRelCenter = endPos.position - centerPoint;


    }
    IEnumerator Move()
    {
        start:
        GetCenter(Vector3.up);
        if (!repeatable)
        {
            float fracComplete = (Time.time - startTime) / journeyTime * speed;
            transform.position = Vector3.Slerp(startRelCenter, endRelCenter, fracComplete * speed);
            transform.position += centerPoint;

        }
        else
        {
            float fracComplete = Mathf.PingPong(Time.time - startTime, journeyTime / speed);
            transform.position = Vector3.Slerp(startRelCenter, endRelCenter, fracComplete * speed);
            transform.position += centerPoint;
            if (fracComplete >= 1)
            {
                startTime = Time.time;
            }
        }
        if (transform.position == endPos.position)
        {       
            yield return null;
        }
        yield return new WaitForSeconds(0.01f);
        goto start;
    }
}
