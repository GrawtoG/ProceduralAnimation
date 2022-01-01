using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNoise : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    public int xSize = 10;
    public int zSize = 10;
    [Range(2, 255)]
    public int verBokLiczba = 11;

    public float heightMultiplier = 1f;

    public float minHeight = float.MaxValue;
    public float maxHeight = float.MinValue;
    float height;

    public int xOffset;
    public int zOffset;

    Vector2[] Uvs;
    Color[] colors;


    public AnimationCurve heightCurve;

    Texture2D texture;



    public float scale = 1f;
    public Gradient gradient;
    public Octave[] elo;
    [System.Serializable]
    public class Octave
    {
        public float Frequency;
        public float Amplitude;
        //public float Scale;
    }
    void Start()
    {
        float[,] noiseMap = GenerateNoiseMap();
    }
    void CreateNoiseMapTexture(float[,] noiseMap)
    {
        texture = new Texture2D(verBokLiczba, verBokLiczba);
        Color[] colorMap = new Color[verBokLiczba * verBokLiczba];
        for (int z = 0, i = 0; z < verBokLiczba; z++)
        {
            for (int x = 0; x < verBokLiczba; x++)
            {
                colorMap[i] = Color.Lerp(Color.black, Color.white, noiseMap[x, z]);
                i++;
            }
        }
        texture.SetPixels(colorMap);
        texture.Apply();
        GetComponent<Renderer>().sharedMaterial.mainTexture = texture;
        transform.localScale = new Vector3(xSize, 1, zSize);
    }
    // Update is called once per frame
    float[,] GenerateNoiseMap()
    {
        //vertices = new Vector3[verBokLiczba * verBokLiczba];
        float[,] noiseMap = new float[verBokLiczba, verBokLiczba];
        for (int z = 0; z < verBokLiczba; z++)
        {
            for (int x = 0; x < verBokLiczba; x++)
            {
                float xVer;
                float zVer;

                xVer = x * ((float)xSize) / (verBokLiczba - 1);
                zVer = z * ((float)zSize) / (verBokLiczba - 1);
                float y = Calculate(xVer, zVer);

                noiseMap[x, z] = y;
                if (y > maxHeight)
                {
                    maxHeight = y;
                }
                if (y < minHeight)
                {
                    minHeight = y;
                }

            }
        }

        for (int z = 0; z < verBokLiczba; z++)
        {
            for (int x = 0; x < verBokLiczba; x++)
            {
                noiseMap[x, z] = Mathf.InverseLerp(minHeight, maxHeight, noiseMap[x, z]);
                noiseMap[x, z] = heightCurve.Evaluate(noiseMap[x, z]) * heightMultiplier;
            }
        }

        return noiseMap;
    }
    float Calculate(float x, float z)
    {
        float xCoord;
        float zCoord;
        float y = 0;
        float perlVal;

        for (int i = 0; i < elo.Length; i++)
        {
            xCoord = (xOffset + x) / scale * elo[i].Frequency;
            zCoord = (zOffset + z) / scale * elo[i].Frequency;
            perlVal = Mathf.PerlinNoise(xCoord, zCoord) * 2 - 1;
            y += perlVal * elo[i].Amplitude;



        }

        return y;
    }
}
