using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class TerrenGeneration : MonoBehaviour
{
    public GameObject chunk;
    public string seed = "";

    public int offsetX;
    public int offsetY;

    int number1;
    int number2;
    void Start()
    {
        if (seed.Length < 18)
        {
            offsetX = Random.Range(0, 999999);
            offsetY = Random.Range(0, 999999);

            
        }

        if (seed.Length > 18)
        {
            seed = seed.Substring(0, 18);
        }

        string seedXString = seed.Substring(0, 9);
        string seedYString = seed.Substring(9, 9);
        if (!int.TryParse(seedYString, out number1) || !int.TryParse(seedXString, out number2))
        {

            offsetX = Random.Range(0, 999999);
            offsetY = Random.Range(0, 999999);
        }
        else
        {

            offsetY = number1;
            offsetX = number2;

        }
    }


}