using UnityEngine;


public class terrainGenerator : MonoBehaviour
{
    public GameObject chunk;
    public GameObject chunkGroupPrefab;
    private GameObject chunkGroupClone;
    public string seed = "";

    public float offsetX;
    public float offsetZ;

    private float chunkOffsetX;
    private float chunkOffsetZ;
    public float chunkSizeX = 10f;
    public float chunkSizeZ= 10f;

    public int chunkLiczba = 10;
    int number1;
    int number2;
    GameObject chunkClone;
    void Start()
    {
        #region Seed
        if (seed.Length < 14)
        {
            offsetX = Random.Range(0f, 9999999f);
            offsetZ = Random.Range(0f, 9999999f);
            goto generate;

        }

        if (seed.Length > 14)
        {
            seed = seed.Substring(0, 14);
        }
        
        string seedXString = seed.Substring(0, 7);
        string seedYString = seed.Substring(7, 7);
        
        
        if (!int.TryParse(seedYString, out number1) || !int.TryParse(seedXString, out number2))
        {

            offsetX = Random.Range(0f, 9999999f);
            offsetZ = Random.Range(0f, 9999999f);
        }
        else
        {

            offsetZ = number1;
            offsetX = number2;

        }
        #endregion
        generate:
        chunkOffsetX = offsetX;
        chunkOffsetZ = offsetZ;
        //chunkGroupClone = Instantiate(chunkGroupPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        for (int z=0,i=0; z<chunkLiczba; z++)
        {
            for (int x = 0; x < chunkLiczba; x++)
            {
                chunkOffsetX += x * chunkSizeX;
                chunkOffsetZ += z * chunkSizeZ;
                chunkClone = Instantiate(chunk, new Vector3(x * chunkSizeX, 0, z * chunkSizeZ), Quaternion.identity);
                if (i % 10 == 0)
                {
                    chunkGroupClone = Instantiate(chunkGroupPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                }
                chunkClone.transform.SetParent(chunkGroupClone.transform);
               
                chunkClone.GetComponent<ChunkScript>().Generate(chunkOffsetX, chunkOffsetZ);
               

                chunkOffsetX = offsetX;
                chunkOffsetZ = offsetZ;
                //chunkClone.SetActive(false);
                i++;
            }

        }
       
    }



}