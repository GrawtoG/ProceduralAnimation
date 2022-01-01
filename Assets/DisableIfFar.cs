using UnityEngine;
using System.Collections;

public class DisableIfFar : MonoBehaviour
{

    // --------------------------------------------------
    // Variables:

    public GameObject itemActivatorObject;
    private ItemActivator activationScript;

    // --------------------------------------------------

    void Awake()
    {
        if (itemActivatorObject == null)
        {
            StartCoroutine("GetParent");
            return;
        }
        StartCoroutine("AddToList");
        

    }

    IEnumerator AddToList()
    {
        yield return new WaitForEndOfFrame();

        activationScript.addList.Add(new ActivatorItem { item = this.gameObject });
    }
    IEnumerator GetParent()
    {
        yield return new WaitUntil(()=>transform.parent != null);
        if (itemActivatorObject == null)
        {
            itemActivatorObject = transform.parent.gameObject;
        }
        activationScript = itemActivatorObject.GetComponent<ItemActivator>();
        StartCoroutine("AddToList");

    }
}