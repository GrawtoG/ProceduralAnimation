using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meshTest : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    public int xSize =10;
    public int zSize=10;
    [Range(2,255)]
    public int verBokLiczba=11;
    float xVer;
    float zVer;
    public int xOffset;
    public int zOffset;

    Vector2[] Uvs;

    public float scale = 1f;
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
        
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateShape();
        UpdateMesh();
    }

    // Update is called once per frame
    void Update()
    {
        CreateShape();
        UpdateMesh();
    }

    void CreateShape()
    {
        vertices = new Vector3[verBokLiczba * verBokLiczba];
        for(int z=0,i = 0; z < verBokLiczba; z++)
        {
            for (int x = 0;x < verBokLiczba; x++)
            {

                xVer = x*((float)xSize)/(verBokLiczba-1);
                zVer = z*((float)zSize)/(verBokLiczba-1);
                vertices[i]=new Vector3(xVer, Calculate(xVer, zVer), zVer);
              
                i++;
            }
        }

        int vert = 0;

        int tris = 0;
        triangles = new int[(verBokLiczba-1)*(verBokLiczba-1)*6];
        for(int z = 0;z< verBokLiczba - 1; z++)
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

        Uvs = new Vector2[vertices.Length];
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                Uvs[i] = new Vector2((float)x / xSize, (float)z / zSize);
                i++;
            }
        }





    }
    void UpdateMesh()
    {
        mesh.Clear();
        
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = Uvs;


        mesh.RecalculateNormals();
    }
    float Calculate(float x, float z)
    {
        float xCoord = (xOffset + x)  * scale;
        float zCoord = (zOffset + z) * scale;
        float y = 0;

        for (int i = 0; i < elo.Length; i++)
        {
            y += Mathf.PerlinNoise(elo[i].Frequency * xCoord, elo[i].Frequency * zCoord) * elo[i].Amplitude;

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
