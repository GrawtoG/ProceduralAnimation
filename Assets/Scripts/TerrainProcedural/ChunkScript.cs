using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkScript : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    public int xSize = 10;
    public int zSize = 10;
    [Range(2, 255)]
    public int verBokLiczba = 11;

    public float heightMultiplier=1f;
    public float OffsetX;
    public float OffsetZ;

    
    public float minHeight = float.MaxValue;
    public float maxHeight = float.MinValue;
    float height;

    Color[] colors;
    Vector2[] Uvs;

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
    public void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }
    public void Generate(float xOffset, float zOffset)
    {
       
        OffsetX = xOffset;
        OffsetZ = zOffset;

        float[,] noiseMap = GenerateNoiseMap();
        CreateShape(noiseMap);
        //CreateNoiseMapTexture(noiseMap);
        UpdateMesh();

    }

    // Update is called once per frame


    void CreateShape(float[,] noiseMap)
    {
        #region Verticles
        vertices = new Vector3[verBokLiczba * verBokLiczba];
        float xVer;
        float zVer;
        for (int z = 0, i = 0; z < verBokLiczba; z++)
        {
            for (int x = 0; x < verBokLiczba; x++)
            {
                xVer = x * ((float)xSize) / (verBokLiczba - 1);
                zVer = z * ((float)zSize) / (verBokLiczba - 1);
                //Debug.Log(noiseMap[x, z]);
                float y = noiseMap[x, z]*heightMultiplier;
                vertices[i] = new Vector3(xVer, y, zVer);

                i++;
            }

        }
        #endregion
        #region Triangles
        int vert = 0;
        int tris = 0;
        triangles = new int[(verBokLiczba - 1) * (verBokLiczba - 1) * 6];
        for (int z = 0; z < verBokLiczba - 1; z++)
        {
            for (int x = 0; x < verBokLiczba - 1; x++)
            {

                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + verBokLiczba;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + verBokLiczba;
                triangles[tris + 5] = vert + verBokLiczba + 1;
                vert++;
                tris += 6;
            }
            vert++;
        }
        #endregion
        #region Uvs
        /*Uvs = new Vector2[vertices.Length];
        for (int i = 0, z = 0; z <verBokLiczba; z++)
        {
            for (int x = 0; x < verBokLiczba; x++)
            {
                float xVer;
                float zVer;
                xVer = x * (((float)xSize) / (verBokLiczba - 1)) / (float)xSize;
                zVer = z * (((float)zSize) / (verBokLiczba - 1)) / (float)zSize;
                Uvs[i] = new Vector2(xVer, zVer);
                i++;
            }
        }*/
        #endregion
        #region Colors
        colors = new Color[vertices.Length];
        for (int i = 0, z = 0; z < verBokLiczba; z++)
        {
            for (int x = 0; x < verBokLiczba; x++)
            {
                height = Mathf.InverseLerp(minHeight, maxHeight, vertices[i].y);
                colors[i] = gradient.Evaluate(height);
                i++;
            }
        }
        #endregion


    }
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

        /*for (int z = 0; z < verBokLiczba; z++)
        {
            for (int x = 0; x < verBokLiczba; x++)
            {
                noiseMap[x, z] = Mathf.InverseLerp(minHeight, maxHeight, noiseMap[x, z]);
                noiseMap[x, z] = heightCurve.Evaluate(noiseMap[x, z]) * heightMultiplier;
            }
        }*/

        return noiseMap;
    }
    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        //mesh.uv = Uvs;
        mesh.colors = colors;

        mesh.RecalculateNormals();
        GetComponent<MeshFilter>().mesh = mesh;
    }
    float Calculate(float x, float z)
    {
        float xCoord;
        float zCoord;
        float y = 0;
        float perlVal;

        for (int i = 0; i < elo.Length; i++)
        {
            xCoord = (OffsetX + x) / scale * elo[i].Frequency;
            zCoord = (OffsetZ + z) / scale * elo[i].Frequency;
            perlVal = Mathf.PerlinNoise(xCoord, zCoord) * 2 - 1;
            y += perlVal * elo[i].Amplitude;



        }

        return y;
    }
    /*private void OnDrawGizmos()
    {
        if (vertices == null)
            return;

        Gizmos.color = Color.yellow;
        for(int i=0; i<vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], .1f);
        }
    }*/
}
