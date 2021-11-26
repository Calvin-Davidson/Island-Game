using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HexagonMesh
{
    private Mesh _mesh;

    public HexagonMesh()
    {
        CreateMeshAsset(1, 1);
    }

    public HexagonMesh(float width, float height)
    {
        CreateMeshAsset(width, height);
    }

    private void CreateMeshAsset(float width, float height)
    {
        Mesh mesh = new Mesh();

        mesh = HexagonMeshBuilder.GetHexagonMesh(width, height);
        mesh.name = "hexagonMesh";
        
        if (!AssetDatabase.IsValidFolder("Assets/Meshes")) AssetDatabase.CreateFolder("Assets", "Meshes");
        AssetDatabase.CreateAsset(mesh, "Assets/Meshes/hexagonMesh.asset");
        AssetDatabase.SaveAssets();

        _mesh = mesh;
    }

    public Mesh Mesh => _mesh;
}
