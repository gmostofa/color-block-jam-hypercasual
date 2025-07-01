using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public int width = 6;
    public int height = 10;
    public float cellSize = 1f;
    public Vector3 offset;

    [Header("Visual Settings")]
    public Color gridColor = Color.green;
    public GameObject tilePrefab; // Visual block for each cell
    public bool generateTiles = true;
    public Material materialA;
    public Material materialB;

    void Start()
    {
        if (generateTiles && tilePrefab != null)
        {
            SpawnVisualGrid();
        }
    }

    void SpawnVisualGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 worldPos = GetWorldPosition(x, y);
                GameObject tile = Instantiate(tilePrefab, worldPos, Quaternion.identity, transform);
                
                // Apply checkerboard pattern
                Renderer rend = tile.GetComponent<Renderer>();
                if (rend != null)
                {
                    Material matToUse = (x + y) % 2 == 0 ? materialA : materialB;
                    rend.material = matToUse;
                }
            }
        }
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return transform.position  + new Vector3(x * cellSize, 0, y * cellSize) + offset;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gridColor;

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