using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;


public class meshTestUI : MonoBehaviour
{
    #region UI 
    public Slider verticesSlider;
    public Text verticesText;
    
    public InputField scaleInput;

    public Toggle autUpToggle;

    public Slider YOffsetSlider;
    public Text YOffsetText;
    public Slider XOffsetSlider;
    public Text XOffsetText;

    public InputField YsizeInput;
    public InputField XsizeInput;

    public InputField dFreqInput;
    public InputField dAmplInput;

    public InputField heightMultInput;


    #endregion


    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    public int xSize = 10;
    public int zSize = 10;
    [Range(2, 255)]
    public int verBokLiczba = 11;
    public int numOctaves = 2;
    public float deltaFreq = 2;
    public float deltaAmpl = 2;
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
    
    [Tooltip("If checked Mesh updates continously")]
    public bool autoupdate = false;
    
    void Start()
    {

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        Generate();

    }
    private void Update()
    {
        verBokLiczba = (int)verticesSlider.value;
        verticesText.text = "Liczba punktów: " + verBokLiczba * verBokLiczba;

        scale = float.Parse(scaleInput.text);

        zOffset = (int)YOffsetSlider.value;
        YOffsetText.text = "OffsetY: " + zOffset;
        xOffset = (int)XOffsetSlider.value;
        XOffsetText.text = "OffsetX: " + xOffset;

        xSize = int.Parse(XsizeInput.text);
        zSize = int.Parse(YsizeInput.text);

        deltaFreq = float.Parse(dFreqInput.text);
        deltaAmpl = float.Parse(dAmplInput.text);

        heightMultiplier = float.Parse(heightMultInput.text);

        if (autUpToggle.isOn)
        {
            autoupdate = true;
        }
        else
        {
            autoupdate = false;
        }
        if (autoupdate)
        {
            Generate();
        }
        
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
    private void FixedUpdate()
    {

        //Generate();

    }
    public void Generate()
    {
        float[,] noiseMap = GenerateNoiseMap();
        CreateShape(noiseMap);
        //CreateNoiseMapTexture(noiseMap);
        UpdateMesh();

    }

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
                float y = noiseMap[x, z];
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
        /*colors = new Color[vertices.Length];
        for (int i = 0, z = 0; z < verBokLiczba; z++)
        {
            for (int x = 0; x < verBokLiczba; x++)
            {
                height = Mathf.InverseLerp(minHeight, maxHeight, vertices[i].y);
                colors[i] = gradient.Evaluate(height);
                i++;
            }
        }*/
        #endregion


    }
    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        // mesh.uv = Uvs;
        mesh.colors = colors;


        mesh.RecalculateNormals();
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
                float y = Calculate(xVer, zVer) * heightMultiplier;

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
            for(int x = 0;x < verBokLiczba; x++)
            {
                noiseMap[x,z] = Mathf.InverseLerp(minHeight, maxHeight, noiseMap[x,z]);
                noiseMap[x,z] = heightCurve.Evaluate(noiseMap[x,z])*heightMultiplier;
            }          
        }*/

        return noiseMap;
    }

    float Calculate(float x, float z)
    {
        float xCoord;
        float zCoord;
        float y = 0;
        float perlVal;
        float freq = 1;
        float ampl = 1;

        for (int i = 0; i < numOctaves; i++)
        {
            xCoord = (xOffset + x) / scale * freq;
            zCoord = (zOffset + z) / scale * freq;
            perlVal = Mathf.PerlinNoise(xCoord, zCoord) * 2 - 1;
            y += perlVal * ampl;
            freq /= deltaFreq;
            ampl /= deltaAmpl;


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



