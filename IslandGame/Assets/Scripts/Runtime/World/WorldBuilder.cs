using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Toolbox.Utility;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(WorldManager))]
public class WorldBuilder : MonoBehaviour
{
    [SerializeField] private float popupAnimationSpeed;

    [SerializeField, Space] private MeshFilter grassTopFilter;
    [SerializeField] private MeshCollider grassTopCollider;
    [SerializeField] private MeshFilter grassWallsFilter;
    [SerializeField] private MeshCollider grassWallsCollider;

    [SerializeField, Space] private MeshFilter sandTopFilter;
    [SerializeField] private MeshCollider sandTopCollider;
    [SerializeField] private MeshFilter sandWallsFilter;
    [SerializeField] private MeshCollider sandWallsCollider;

    [SerializeField, Space] private GameObject treeRenderer;


    private Queue<Func<Task>> _meshCombineQueue = new Queue<Func<Task>>();
    private Coroutine _queueRunner;
    private WorldManager _worldManager;

    private void Awake()
    {
        _worldManager = GetComponent<WorldManager>();
        grassTopFilter.mesh = new Mesh();
        grassWallsFilter.mesh = new Mesh();
        sandTopFilter.mesh = new Mesh();
        sandWallsFilter.mesh = new Mesh();


        grassTopCollider.sharedMesh = grassTopFilter.sharedMesh;
        grassWallsCollider.sharedMesh = grassWallsCollider.sharedMesh;

        sandTopCollider.sharedMesh = sandTopFilter.sharedMesh;
        sandWallsCollider.sharedMesh = sandWallsCollider.sharedMesh;
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
        throw new NotImplementedException("The DeleteTile methode has not been implemented!");
        return false;
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

        _meshCombineQueue.Enqueue(() => CombineMesh(tile));
        _queueRunner ??= StartCoroutine(TaskQueueRunner());
    }

    private async Task CombineMesh(GameObject tile)
    {
        await AddHexagonToMesh(grassTopFilter.mesh, new Vector2(1, 1), tile.transform.position);
        grassTopFilter.mesh.Optimize();

        grassTopCollider.sharedMesh = grassTopFilter.mesh;
        Destroy(tile.transform.Find("Grass").gameObject);
        Destroy(tile.transform.Find("Dirt").gameObject);

        CombineTrees(tile);
    }

    private void CombineTrees(GameObject tile)
    {
        var meshFilters = tile.GetComponentsInChildren<MeshFilter>();

            meshFilters = meshFilters.ToList().FindAll(filter => filter.sharedMesh.name == "tree").ToArray();

            GameObject[] objects = new GameObject[meshFilters.Length];
            for (var i = meshFilters.Length - 1; i >= 0; i--) objects[i] = meshFilters[i].gameObject;

            MyMeshUtility.CombineMeshes(treeRenderer, objects.ToList());
    }

    private async Task AddHexagonToMesh(Mesh parentMesh, Vector2 size, Vector3 position)
    {
        List<int> triangles = new List<int>(parentMesh.triangles);
        List<Vector3> vertices = new List<Vector3>(parentMesh.vertices);
        List<Vector3> normals = new List<Vector3>(parentMesh.normals);

        await Task.Run(() =>
        {
            float width = size.x;
            float height = size.y;

            float offsetY = 5.4f;

            Vector3 vert0 = new Vector3(-width + position.x, height / 2 - offsetY, 0 + position.z);
            Vector3 vert1 = new Vector3(-width / 2 + position.x, height / 2 - offsetY, width + position.z);
            Vector3 vert2 = new Vector3(width / 2 + position.x, height / 2 - offsetY, width + position.z);
            Vector3 vert3 = new Vector3(width + position.x, height / 2 - offsetY, 0 + position.z);
            Vector3 vert4 = new Vector3(width / 2 + position.x, height / 2 - offsetY, -width + position.z);
            Vector3 vert5 = new Vector3(-width / 2 + position.x, height / 2 - offsetY, -width + position.z);
            Vector3 vert6 = new Vector3(-width + position.x, -height / 2 - offsetY, 0 + position.z);
            Vector3 vert7 = new Vector3(-width / 2 + position.x, -height / 2 - offsetY, width + position.z);
            Vector3 vert8 = new Vector3(width / 2 + position.x, -height / 2 - offsetY, width + position.z);
            Vector3 vert9 = new Vector3(width + position.x, -height / 2 - offsetY, 0 + position.z);
            Vector3 vert10 = new Vector3(width / 2 + position.x, -height / 2 - offsetY, -width + position.z);
            Vector3 vert11 = new Vector3(-width / 2 + position.x, -height / 2 - offsetY, -width + position.z);

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


            Vector2 position2d = new Vector2(position.x, position.z);
            if (_worldManager.IsEmptyTile(_worldManager.GetTopLeftCoords(position2d)))
            {
                triangles.AddRange(new[]
                {
                    vertices.IndexOf(vert7), vertices.IndexOf(vert1), vertices.IndexOf(vert0),
                    vertices.IndexOf(vert0), vertices.IndexOf(vert6), vertices.IndexOf(vert7),
                });
            }

            if (_worldManager.IsEmptyTile(_worldManager.GetTopCoords(position2d)))
            {
                triangles.AddRange(new[]
                {
                    vertices.IndexOf(vert8), vertices.IndexOf(vert2), vertices.IndexOf(vert1),
                    vertices.IndexOf(vert1), vertices.IndexOf(vert7), vertices.IndexOf(vert8),
                });
            }

            if (_worldManager.IsEmptyTile(_worldManager.GetTopRightCoords(position2d)))
            {
                triangles.AddRange(new[]
                {
                    vertices.IndexOf(vert9), vertices.IndexOf(vert3), vertices.IndexOf(vert2),
                    vertices.IndexOf(vert2), vertices.IndexOf(vert8), vertices.IndexOf(vert9),
                });
            }


            if (_worldManager.IsEmptyTile(_worldManager.GetBottomLeftCoords(position2d)))
            {
                triangles.AddRange(new[]
                {
                    vertices.IndexOf(vert6), vertices.IndexOf(vert0), vertices.IndexOf(vert5),
                    vertices.IndexOf(vert5), vertices.IndexOf(vert11), vertices.IndexOf(vert6)
                });
            }


            if (_worldManager.IsEmptyTile(_worldManager.GetBottomCoords(position2d)))
            {
                triangles.AddRange(new[]
                {
                    vertices.IndexOf(vert11), vertices.IndexOf(vert5), vertices.IndexOf(vert4),
                    vertices.IndexOf(vert4), vertices.IndexOf(vert10), vertices.IndexOf(vert11),
                });
            }

            if (_worldManager.IsEmptyTile(_worldManager.GetBottomRightCoords(position2d)))
            {
                triangles.AddRange(new[]
                {
                    vertices.IndexOf(vert10), vertices.IndexOf(vert4), vertices.IndexOf(vert3),
                    vertices.IndexOf(vert3), vertices.IndexOf(vert9), vertices.IndexOf(vert10),
                });
            }


            triangles.AddRange(new[]
            {
                // top plane
                vertices.IndexOf(vert0), vertices.IndexOf(vert1), vertices.IndexOf(vert5),
                vertices.IndexOf(vert4), vertices.IndexOf(vert5), vertices.IndexOf(vert1),
                vertices.IndexOf(vert1), vertices.IndexOf(vert2), vertices.IndexOf(vert4),
                vertices.IndexOf(vert3), vertices.IndexOf(vert4), vertices.IndexOf(vert2),
            });

            normals.AddRange(new[]
            {
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


        parentMesh.vertices = vertices.ToArray();
        parentMesh.triangles = triangles.ToArray();
        parentMesh.normals = normals.ToArray();

        grassTopFilter.mesh.Optimize();
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