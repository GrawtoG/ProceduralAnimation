using UnityEngine;
using System.Collections;

// Create a texture and fill it with Perlin noise.
// Try varying the xOrg, yOrg and scale values in the inspector
// while in Play mode to see the effect they have on the noise.

public class asf : MonoBehaviour
{

    Vector3[] vertices;
    int[] triangles;
    // Width and height of the texture in pixels.
    public int pixWidth;
    public int pixHeight;

    // The origin of the sampled area in the plane.
    public float offsetX;
    public float offsetY;

    // The number of cycles of the basic noise pattern that are repeated
    // over the width and height of the texture.
    public float scale = 1.0F;

    Color[] colors;
    private Texture2D noiseTex;
    private Color[] pix;
    private Renderer rend;
    [SerializeField]
    public Octave[] elo;

    Mesh mesh;
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
       // GetComponent<MeshFilter>().mesh = mesh;

        offsetX = Random.Range(0f, 99999f);
        offsetY = Random.Range(0f, 99999f);
        rend = GetComponent<Renderer>();

        // Set up the texture and a Color array to hold pixels during processing.
        noiseTex = new Texture2D(pixWidth, pixHeight);
        pix = new Color[noiseTex.width * noiseTex.height];
        rend.material.mainTexture = noiseTex;

    }

    void CalcNoise()
    {
        // For each pixel in the texture...
        float y = 0.0F;

        while (y < noiseTex.height)
        {
            float x = 0.0F;
            while (x < noiseTex.width)
            {
                float xCoord = offsetX + x / noiseTex.width * scale;
                float yCoord = offsetY + y / noiseTex.height * scale;
                float sample = CalculateColor(xCoord, yCoord);
                pix[(int)y * noiseTex.width + (int)x] = new Color(sample, sample, sample);
                x++;
            }
            y++;
        }

        // Copy the pixel data to the texture and load it into the GPU.
        noiseTex.SetPixels(pix);
        noiseTex.Apply();
    }
    float CalculateColor(float x, float z)
    {

        float y = 0;

        for (int i = 0; i < elo.Length; i++)
        {
            y += Mathf.PerlinNoise(elo[i].Frequency * x, elo[i].Frequency * z) * elo[i].Amplitude*scale;

        }

        return y;
    }

    void CreateShape()
    {
        #region Vertices
        vertices = new Vector3[(pixWidth + 1) * (pixHeight + 1)];

        for (int i = 0, z = 0; z <= pixWidth; z++)
        {
            for (int x = 0; x <= pixHeight; x++)
            {
                float y = CalculateColor(x, z);
                vertices[i] = new Vector3(x, y, z);

                i++;
            }
        }

        int vert = 0;
        int tris = 0;
        #endregion
        #region Triangles
        triangles = new int[pixWidth * pixHeight * 6];

        for (int z = 0; z < pixHeight; z++)
        {
            for (int x = 0; x < pixWidth; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + pixWidth + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + pixWidth + 1;
                triangles[tris + 5] = vert + pixWidth + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }
        #endregion
    }
    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;
        //mesh.uv = Uvs;

        mesh.RecalculateNormals();

    }
    void Update()
    {
        
            CalcNoise();
            //CreateShape();

            //UpdateMesh();
        

    }
    private void Awake()
    {
        
    }
    
}
