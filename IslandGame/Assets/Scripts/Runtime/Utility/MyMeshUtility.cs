using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Toolbox.Utility
{
    public class MyMeshUtility
    {
        private static CombineInstance[] CombineInstanceBuilder(MeshFilter[] filters)
        {
            CombineInstance[] combine = new CombineInstance[filters.Length];
            for (int i = 0; i < filters.Length; i++)
            {
                if (filters[i] is null) continue;
                combine[i].subMeshIndex = 0;
                combine[i].mesh = filters[i].mesh;
                combine[i].transform = filters[i].transform.localToWorldMatrix;
            }
            return combine;
        }
        public static bool CombineMeshes(GameObject parentObject, List<GameObject> gameObjects)
        {
            if (gameObjects.Count == 0) return false;

            List<MeshFilter> filters = gameObjects.Select(o => o.GetComponent<MeshFilter>()).ToList();

            MeshFilter meshFilter = parentObject.GetComponent<MeshFilter>() == null ? parentObject.AddComponent<MeshFilter>() : parentObject.GetComponent<MeshFilter>();

            filters.Add(meshFilter);

            var meshInstances = CombineInstanceBuilder(filters.ToArray());
            meshFilter.mesh = new Mesh();
            meshFilter.mesh.CombineMeshes(meshInstances);
            meshFilter.mesh.Optimize();
            

            foreach (var obj in gameObjects)
            {
                if (Application.isPlaying) Object.Destroy(obj);
                else
                {
                    Object.DestroyImmediate(obj);
                }
            }

            return true;
        }
    }
}