using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [Header("Grid Settings")]
    public int width = 6;
    public int height = 10;
    public float cellSize = 1f;
    public Vector3 offset;

    [Header("Visual Settings")]
    public GameObject tilePrefab;
    public Material materialA;
    public Material materialB;
    
    private Dictionary<Vector2Int, BlockController> occupiedTiles = new();

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SpawnVisualGrid();
    }

    public void SpawnVisualGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 pos = GetWorldPosition(x, y);
                GameObject tile = Instantiate(tilePrefab, pos, Quaternion.identity, transform);

                // Apply checkerboard material
                Renderer rend = tile.GetComponent<Renderer>();
                if (rend != null)
                {
                    Material chosen = (x + y) % 2 == 0 ? materialA : materialB;
                    rend.material = chosen;
                }

                tile.name = $"Tile {x},{y}";
            }
        }
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return transform.position + new Vector3(x * cellSize, 0, y * cellSize) + offset;
    }

    public bool IsInsideGrid(Vector2Int pos)
    {
        return pos.x >= 0 && pos.y >= 0 && pos.x < width && pos.y < height;
    }
    public void RegisterBlock(BlockController block)
    {
        foreach (Vector2Int pos in block.GetOccupiedTiles())
        {
            occupiedTiles[pos] = block;
        }
    }

    public void UnregisterBlock(BlockController block)
    {
        foreach (Vector2Int pos in block.GetOccupiedTiles())
        {
            if (occupiedTiles.ContainsKey(pos) && occupiedTiles[pos] == block)
                occupiedTiles.Remove(pos);
        }
    }

    public bool IsTileOccupied(Vector2Int pos, BlockController ignore = null)
    {
        return occupiedTiles.ContainsKey(pos) && occupiedTiles[pos] != ignore;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        for (int x = 0; x <= width; x++)
        {
            Vector3 from = transform.position + new Vector3(x * cellSize, 0, 0);
            Vector3 to = transform.position + new Vector3(x * cellSize, 0, height * cellSize);
            Gizmos.DrawLine(from, to);
        }

        for (int y = 0; y <= height; y++)
        {
            Vector3 from = transform.position + new Vector3(0, 0, y * cellSize);
            Vector3 to = transform.position + new Vector3(width * cellSize, 0, y * cellSize);
            Gizmos.DrawLine(from, to);
        }
    }
}
