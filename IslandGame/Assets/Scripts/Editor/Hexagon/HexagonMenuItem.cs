using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class HexagonMenuItem : EditorWindow
{
    [MenuItem("GameObject/3D Object/Hexagon", false, 10)]
    public static void GenerateHexagon()
    {
        HexagonMesh hexagonMesh = new HexagonMesh();
        GameObject gameObject = new GameObject("Hexagon", typeof(MeshRenderer), typeof(MeshFilter));
        gameObject.GetComponent<MeshFilter>().mesh = hexagonMesh.Mesh;
    }
}
