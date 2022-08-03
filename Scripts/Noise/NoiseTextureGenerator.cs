using UnityEngine;

public class NoiseTextureGenerator : MonoBehaviour
{
    public int textureResolution = 256;
    public int width = 100;
    public int height = 100;
    public int seed;
    public float scale;
    public Vector2 offset;
    [Range(1, 8)]
    public int octaves;
    [Range(0f, 1f)]
    public float persistance;
    public float lacunarity;

    private Texture2D texture;
    private Material meshMaterial;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        Init();
    }

    private void OnValidate()
    {
        Init();
    }

    private void Init()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        //CreateTexture();
        CreateMesh();
    }

    private void CreateMesh()
    {
        float[,] heightMap = Noise.GetHeightMap(width, height, scale, seed, octaves, persistance, lacunarity, offset);
        Mesh mesh = MeshGenerator.GenerateTerrain(heightMap);

        meshFilter.sharedMesh = mesh;
    }

    private void CreateTexture(float[,] heightMap)
    {
        if (texture == null)
        {
            texture = new Texture2D(textureResolution, textureResolution, TextureFormat.RGB24, true);
            texture.name = "Noise Texture";
            texture.wrapMode = TextureWrapMode.Clamp;
            GetComponent<MeshRenderer>().sharedMaterial.mainTexture = texture;
        }
        FillTexture(heightMap);
    }

    public void FillTexture(float[,] heightMap)
    {
        Color[] colors = GenerateColorMap(heightMap);

        texture.SetPixels(colors);
        texture.Apply();
    }

    private Color[] GenerateColorMap(float[,] heightMap)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        Color[] colors = new Color[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float sample = heightMap[x, y];
                Color color = new Color(sample, sample, sample);

                colors[y * textureResolution + x] = color;
            }
        }

        return colors;
    }

    //private void OnDrawGizmos()
    //{
    //    Vector3[] vertices = meshFilter.sharedMesh.vertices;
    //    if (vertices != null)
    //    {
    //        Gizmos.color = Color.black;
    //        for (int i = 0; i < vertices.Length; i++)
    //        {
    //            Gizmos.DrawSphere(vertices[i], 0.1f);
    //        }
    //    }
    //}
}
