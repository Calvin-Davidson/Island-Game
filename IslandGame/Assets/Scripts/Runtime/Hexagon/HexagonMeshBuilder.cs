using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonMeshBuilder
{
    public static Mesh GetHexagonMesh(float width, float height)
    {
        Mesh mesh = new Mesh
        {
            vertices = GetVertices(width, height),
            triangles = GetTriangles(),
            uv = GetUvs(),
            normals = GetNormals()
        };

        return mesh;
    }

    private static int[] GetTriangles()
    {
        return new []
        {
            // top plane
            0, 1, 5,
            4, 5, 1,
            1, 2, 4,
            3, 4, 2,
            // bottom plane
            11, 7, 6,
            7, 11, 10,
            10, 8, 7,
            8, 10, 9,
            // side 0
            7, 1, 0,
            0, 6, 7,
            // side 1
            8, 2, 1,
            1, 7, 8,
            // side 2
            9, 3, 2,
            2, 8, 9,
            // side 3
            10, 4, 3,
            3, 9, 10,
            // side 4
            11, 5, 4,
            4, 10, 11,
            // side 5
            6, 0, 5,
            5, 11, 6
        };
    }

    private static Vector3[] GetVertices(float width, float height)
    {
        return new [] {
            //top plane
            new Vector3(-width, height/2, 0),
            new Vector3(-width/2, height/2, width),
            new Vector3(width/2, height/2, width),
            new Vector3(width, height/2, 0),
            new Vector3(width/2, height/2, -width),
            new Vector3(-width/2, height/2, -width),
            //bottom plane
            new Vector3(-width, -height/2, 0),
            new Vector3(-width/2, -height/2, width),
            new Vector3(width/2, -height/2, width),
            new Vector3(width, -height/2, 0),
            new Vector3(width/2, -height/2, -width),
            new Vector3(-width/2, -height/2, -width)
        };
    }

    private static Vector2[] GetUvs()
    {
        return new []
        {
            new Vector2(0, 0.5f),
            new Vector2(0.33f, 1),
            new Vector2(0.66f, 1),
            new Vector2(1, 0.5f),
            new Vector2(0.66f, 0),
            new Vector2(0.33f, 0),
            //
            new Vector2(1, 0.5f),
            new Vector2(0.66f, 0),
            new Vector2(0.33f, 0),
            new Vector2(0, 0.5f),
            new Vector2(0.33f, 1),
            new Vector2(0.66f, 1),
        };
    }

    private static Vector3[] GetNormals()
    {
        return new [] {
            //top plane
            Vector3.up,
            Vector3.up,
            Vector3.up,
            Vector3.up,
            Vector3.up,
            Vector3.up,
            //bottom plane
            Vector3.up,
            Vector3.up,
            Vector3.up,
            Vector3.up,
            Vector3.up,
            Vector3.up
        };
    }
}
