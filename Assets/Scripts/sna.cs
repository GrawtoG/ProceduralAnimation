using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sna : MonoBehaviour
{

    public List<Transform> bodyParts = new List<Transform>();
    public int elements = 1;
    public float minDistance = 0.25f;

    public int beginSize;

    public float speed = 1;
    public float rotationSpeed = 50;
    public float quaternionSpeed = 1;
    public GameObject bodyprefabs;

    private float dis;
    private Transform curBodyPart;
    private Transform PrevBodyPart;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < beginSize - 1; i++)
        {

            AddBodyPart();

        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RotateParts();
        if (Input.GetKey(KeyCode.W))
            Move();

        if (Input.GetKeyDown(KeyCode.Q))
            AddBodyPart();
        if (Input.GetKeyDown(KeyCode.E))
            DelBodyPart();
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, , step);
    }
    public void RotateParts()
    {
        
        for (int i = 1; i < bodyParts.Count; i++)
        {
            //bodyParts[i].rotation = Quaternion.RotateTowards(bodyParts[i].rotation, bodyParts[i - 1].rotation, quaternionSpeed * Time.deltaTime * 50);
            //bodyParts[i].LookAt(bodyParts[i-1].position);
            //bodyParts[i].forward = (bodyParts[i - 1].position - bodyParts[i].position).normalized;
            
        }

    }
    public void Move()
    {

        float curspeed = speed;

        if (Input.GetKey(KeyCode.LeftShift))
            curspeed *= 2;

        bodyParts[0].Translate(bodyParts[0].forward * curspeed * Time.smoothDeltaTime, Space.World);

        if (Input.GetAxis("Horizontal") != 0)
            bodyParts[0].Rotate(Vector3.up * rotationSpeed * Time.deltaTime * Input.GetAxis("Horizontal"));

        for (int i = 1; i < bodyParts.Count; i++)
        {

            curBodyPart = bodyParts[i];
            PrevBodyPart = bodyParts[i - 1];

            dis = Vector3.Distance(PrevBodyPart.position, curBodyPart.position);

            Vector3 newpos = PrevBodyPart.position;

            newpos.y = bodyParts[0].position.y;

            float T = Time.deltaTime * dis / minDistance * curspeed;

            if (T > 0.5f)
                T = 0.5f;
            curBodyPart.position = Vector3.Slerp(curBodyPart.position, newpos, T);
           // curBodyPart.rotation = Quaternion.Slerp(curBodyPart.rotation, PrevBodyPart.rotation, T);



        }
    }


    public void AddBodyPart()
    {

        Transform newpart = (Instantiate(bodyprefabs, bodyParts[bodyParts.Count - 1].position, bodyParts[bodyParts.Count - 1].rotation) as GameObject).transform;

        newpart.SetParent(transform);

        bodyParts.Add(newpart);
    }
    public void DelBodyPart()
    {
        

        
        
        
        Component[] stonLegMoveScripts = bodyParts[bodyParts.Count-1].GetComponentsInChildren<stonLegMove>();
        foreach(stonLegMove stonlegmove in stonLegMoveScripts)
        {
            stonlegmove.targetTransform.gameObject.GetComponent<DestroyIKTarget>().Destroy();
        }
        Destroy(bodyParts[bodyParts.Count - 1].gameObject);
        bodyParts.RemoveAt(bodyParts.Count - 1);







    }
   

}