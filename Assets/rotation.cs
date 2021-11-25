using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotation : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    bool counterclockwise; // rotation left or right?

    [SerializeField]
    private int speed;

    private void Start()
    {
        if (counterclockwise)
            speed = -speed;
    }

    private void Update()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.up, out hit, 10);
        
        transform.Rotate(new Vector3(0, speed, 0));
        if (hit.collider != null)
        {
            hit.transform.Rotate(new Vector3(0, speed, 0));
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        col.transform.parent = transform; // keep the player as a child, to rotate him with the platform rotation
    }

    private void OnTriggerExit(Collider col)
    {
        col.transform.parent = null; // remove the player as a child
    }
}
