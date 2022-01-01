using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemActivator : MonoBehaviour
{

    // --------------------------------------------------
    // Variables:

    [SerializeField]
    private int distanceFromPlayer;

    public Transform player;
    public int chunkSizeX;
    public int chunkSizeZ;
    private float halfChSizeX;
    private float halfChSizeZ;
    private List<ActivatorItem> activatorItems;
    [SerializeField]
    public List<ActivatorItem> addList;
    List<ActivatorItem> removeList = new List<ActivatorItem>();

    // --------------------------------------------------

    void Start()
    {
        halfChSizeX = chunkSizeX/2f;
        halfChSizeZ = chunkSizeZ/2f;
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }
        
        activatorItems = new List<ActivatorItem>();
        addList = new List<ActivatorItem>();

        AddToList();
    }

    void AddToList()
    {
        if (addList.Count > 0)
        {
            foreach (ActivatorItem item in addList)
            {
                if (item.item != null)
                {
                    activatorItems.Add(item);
                }
            }

            addList.Clear();
        }

        StartCoroutine("CheckActivation");
    }

    IEnumerator CheckActivation()
    {
        

        if (activatorItems.Count > 0)
        {
            foreach (ActivatorItem item in activatorItems)
            {
                if ((new Vector2(player.position.x,player.position.z)-new Vector2(item.item.transform.position.x+halfChSizeX, item.item.transform.position.z + halfChSizeZ)).sqrMagnitude > distanceFromPlayer*distanceFromPlayer)
                {
                    if (item.item == null)
                    {
                        removeList.Add(item);
                    }
                    else
                    {
                        item.item.SetActive(false);
                    }
                }
                else
                {
                    if (item.item == null)
                    {
                        removeList.Add(item);
                    }
                    else
                    {
                        item.item.SetActive(true);
                    }
                }

                yield return new WaitForEndOfFrame();
            }
        }

        yield return new WaitForEndOfFrame();

        if (removeList.Count > 0)
        {
            foreach (ActivatorItem item in removeList)
            {
                activatorItems.Remove(item);
            }
        }

        yield return new WaitForEndOfFrame();

        AddToList();
    }
    
}

public class ActivatorItem
{
    public GameObject item;
}
