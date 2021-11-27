using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(WorldManager))]
public class WorldBuilder : MonoBehaviour
{
    [SerializeField] private float popupAnimationSpeed;
    [SerializeField] private MeshRenderer worldRenderer;
    [SerializeField] private MeshFilter worldFilter;

    private Queue<Func<Task>> _meshCombineQueue = new Queue<Func<Task>>();
    private Coroutine _queueRunner;
    private WorldManager _worldManager;

    private void Awake()
    {
        _worldManager = GetComponent<WorldManager>();
    }

    public GameObject CreateTile(Vector2 spawnLocation)
    {
        if (_worldManager.Hexagons.ContainsKey(spawnLocation)) return null;

        GameObject hexTile = GameObject.Instantiate(_worldManager.GrassTiles.GetRandom());
        hexTile.transform.position = new Vector3(spawnLocation.x, 0, spawnLocation.y);
        _worldManager.Hexagons.Add(spawnLocation, hexTile);

        StartCoroutine(TilePopupAnimation(hexTile));
        return hexTile;
    }

    public GameObject TryCreateTile(Vector2 spawnLocation)
    {
        if (!_worldManager.IsEmptyTile(spawnLocation)) return null;
        return CreateTile(spawnLocation);
    }


    public bool DeleteTile(Vector3 spawnLocation)
    {
        if (!(_worldManager.Hexagons.ContainsKey(spawnLocation))) return false;

        GameObject obj = _worldManager.Hexagons[new Vector2(spawnLocation.x, spawnLocation.z)];
        GameObject.Destroy(obj);
        return true;
    }

    private IEnumerator TilePopupAnimation(GameObject tile)
    {
        Vector3 targetPosition = tile.transform.position;
        tile.transform.position = targetPosition - new Vector3(0, 1, 0);

        float progress = 0;
        while (progress < 1)
        {
            progress += Time.deltaTime * popupAnimationSpeed;
            progress = progress > 0.8 ? 1 : progress;
            tile.transform.position = Vector3.Slerp(tile.transform.position, targetPosition, progress);
            yield return null;
        }

        _meshCombineQueue.Enqueue(() => CombineMesh(worldFilter.mesh, tile));
        _queueRunner ??= StartCoroutine(TaskQueueRunner());
    }
    
    private async Task CombineMesh(Mesh parentMesh, GameObject tile)
    {
        Vector3 tilePos = tile.transform.position;

        Mesh worldMesh = worldFilter.mesh;
        List<int> triangles = new List<int>(worldMesh.triangles);
        List<Vector3> vertices = new List<Vector3>(worldMesh.vertices);
        List<Vector3> normals = new List<Vector3>(worldMesh.normals);

        await Task.Run(() =>
        {
            float width = 1;
            float height = 1;

            float offsetY = 5.4f;
            
            Vector3 vert0 = new Vector3(-width + tilePos.x, height / 2 - offsetY, +tilePos.z);
            Vector3 vert1 = new Vector3(-width / 2 + tilePos.x, height / 2 - offsetY, width + tilePos.z);
            Vector3 vert2 = new Vector3(width / 2 + tilePos.x, height / 2 - offsetY, width + tilePos.z);
            Vector3 vert3 = new Vector3(width + tilePos.x, height / 2 - offsetY, 0 + tilePos.z);
            Vector3 vert4 = new Vector3(width / 2 + tilePos.x, height / 2 - offsetY, -width + tilePos.z);
            Vector3 vert5 = new Vector3(-width / 2 + tilePos.x, height / 2 - offsetY, -width + tilePos.z);
            Vector3 vert6 = new Vector3(-width + tilePos.x, -height / 2 - offsetY, 0 + tilePos.z);
            Vector3 vert7 = new Vector3(-width / 2 + tilePos.x, -height / 2 - offsetY, width + tilePos.z);
            Vector3 vert8 = new Vector3(width / 2 + tilePos.x, -height / 2 - offsetY, width + tilePos.z);
            Vector3 vert9 = new Vector3(width + tilePos.x, -height / 2 - offsetY, 0 + tilePos.z);
            Vector3 vert10 = new Vector3(width / 2 + tilePos.x, -height / 2 - offsetY, -width + tilePos.z);
            Vector3 vert11 = new Vector3(-width / 2 + tilePos.x, -height / 2 - offsetY, -width + tilePos.z);
            
            vertices.AddRange(new[]
             {
                 //top plane
                 vert0,
                 vert1,
                 vert2,
                 vert3,
                 vert4,
                 vert5,
                 //bottom plane
                 vert6,
                 vert7,
                 vert8,
                 vert9,
                 vert10,
                 vert11
             });

            triangles.AddRange(new[]
            {
                // top plane
                vertices.IndexOf(vert0), vertices.IndexOf(vert1), vertices.IndexOf(vert5),
                vertices.IndexOf(vert4), vertices.IndexOf(vert5), vertices.IndexOf(vert1),
                vertices.IndexOf(vert1), vertices.IndexOf(vert2), vertices.IndexOf(vert4),
                vertices.IndexOf(vert3), vertices.IndexOf(vert4), vertices.IndexOf(vert2),
                // bottom plane
                // vertices.IndexOf(vert11), vertices.IndexOf(vert7), vertices.IndexOf(vert6),
                // vertices.IndexOf(vert7), vertices.IndexOf(vert11), vertices.IndexOf(vert10),
                // vertices.IndexOf(vert10), vertices.IndexOf(vert9), vertices.IndexOf(vert7),
                // vertices.IndexOf(vert8), vertices.IndexOf(vert10), vertices.IndexOf(vert9),
                // side 0
                vertices.IndexOf(vert7), vertices.IndexOf(vert1), vertices.IndexOf(vert0),
                vertices.IndexOf(vert0), vertices.IndexOf(vert6), vertices.IndexOf(vert7),
                // side 1
                vertices.IndexOf(vert8), vertices.IndexOf(vert2), vertices.IndexOf(vert1),
                vertices.IndexOf(vert1), vertices.IndexOf(vert7), vertices.IndexOf(vert8),
                // side 2
                vertices.IndexOf(vert9), vertices.IndexOf(vert3), vertices.IndexOf(vert2),
                vertices.IndexOf(vert2), vertices.IndexOf(vert8), vertices.IndexOf(vert9),
                // side 3
                vertices.IndexOf(vert10), vertices.IndexOf(vert4), vertices.IndexOf(vert3),
                vertices.IndexOf(vert3), vertices.IndexOf(vert9), vertices.IndexOf(vert10),
                // side 4
                vertices.IndexOf(vert11), vertices.IndexOf(vert5), vertices.IndexOf(vert4),
                vertices.IndexOf(vert4), vertices.IndexOf(vert10), vertices.IndexOf(vert11),
                // side 5
                vertices.IndexOf(vert6), vertices.IndexOf(vert0), vertices.IndexOf(vert5),
                vertices.IndexOf(vert5), vertices.IndexOf(vert11), vertices.IndexOf(vert6)
            });
            
            normals.AddRange(new [] {
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
            });
        });
        

        worldFilter.mesh.vertices = vertices.ToArray();
        worldFilter.mesh.triangles = triangles.ToArray();
        worldFilter.mesh.normals = normals.ToArray();
        
        Destroy(tile.transform.Find("Grass").gameObject);
        Destroy(tile.transform.Find("Dirt").gameObject);
    }

    private IEnumerator TaskQueueRunner()
    {
        while (_meshCombineQueue.Count > 0)
        {
            var currentTask = _meshCombineQueue.Dequeue();
            var task = currentTask();
            while (!task.IsCompleted)
            {
                yield return null;
            }
        }

        _queueRunner = null;
    }
}