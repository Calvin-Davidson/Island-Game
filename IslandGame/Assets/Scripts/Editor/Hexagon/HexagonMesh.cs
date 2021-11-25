using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonMesh
{
    private Mesh _mesh;
    public HexagonMesh()
    {
        _mesh = HexagonMeshBuilder.GetHexagonMesh(1, 1);
    }

    public HexagonMesh(float width, float height)
    {
        _mesh = HexagonMeshBuilder.GetHexagonMesh(width, height);
    }

    public Mesh Mesh => _mesh;
}
