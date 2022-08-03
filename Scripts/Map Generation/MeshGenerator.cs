using UnityEngine;

public static class MeshGenerator
{
    public static Mesh GenerateTerrain(float[,] heightMap)
    {
        int height = heightMap.GetLength(0);
        int width = heightMap.GetLength(1);

        Mesh mesh = new Mesh();
        mesh.name = "Terrain Mesh";
        Vector3[] vertices = new Vector3[(width + 1) * (height + 1)];
        Vector2[] uvs = new Vector2[vertices.Length];
        Vector4[] tangents = new Vector4[vertices.Length];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);
        for (int y = 0, i = 0; y <= height; y++)
        {
            for (int x = 0; x <= width; x++, i++)
            {
                vertices[i] = new Vector3(x, y);
                uvs[i] = new Vector2((float)x / width, (float)y / height);
                tangents[i] = tangent;
            }
        }

        int[] triangles = new int[width * height * 6];
        for (int ti = 0, vi = 0, y = 0; y < height; y++, vi++)
        {
            for (int x = 0; x < width; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + width + 1;
                triangles[ti + 5] = vi + width + 2;
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.tangents = tangents;
        mesh.triangles = triangles;

        return mesh;
    }
}
