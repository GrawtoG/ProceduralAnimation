using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyIKTarget : MonoBehaviour
{
    // Start is called before the first frame update
    public sna snaScript;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Destroy()
    {
        Debug.Log("bum " + gameObject.name);
        DestroyImmediate(gameObject);
    }
}
