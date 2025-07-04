using System.Collections.Generic;
using UnityEngine;

public class DoorTile : MonoBehaviour
{
    public Vector2Int gridPosition;
    public Vector2Int size = Vector2Int.one;
    public Color gateColor = Color.white;

    public GameObject visualTilePrefab; // Set in inspector
    private List<GameObject> visualTiles = new();

    void Start()
    {
        CreateVisualTiles();
    }

    void CreateVisualTiles()
    {
        if (!visualTilePrefab)
        {
            Debug.LogWarning("No visualTilePrefab assigned on DoorTile.");
            return;
        }

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Vector2Int tilePos = gridPosition + new Vector2Int(x, y);
                Vector3 worldPos = GridManager.Instance.GetWorldPosition(tilePos.x, tilePos.y);
                worldPos += Vector3.up * 0.05f; // Slight offset above grid

                GameObject tile = Instantiate(visualTilePrefab, worldPos, Quaternion.identity, transform);
                tile.transform.localScale = new Vector3(tile.transform.localScale.x, tile.transform.localScale.y, tile.transform.localScale.z) * 0.9f;
                ApplyColor(tile, gateColor);
                visualTiles.Add(tile);
            }
        }
    }

    public List<Vector2Int> GetOccupiedTiles()
    {
        List<Vector2Int> tiles = new();
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                tiles.Add(gridPosition + new Vector2Int(x, y));
            }
        }
        return tiles;
    }

    private void ApplyColor(GameObject go, Color color)
    {
        Renderer renderer = go.GetComponent<Renderer>();
        if (!renderer) return;

        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        renderer.GetPropertyBlock(mpb);
        mpb.SetColor("_BaseColor", color); // works for Unlit or legacy shaders
        renderer.SetPropertyBlock(mpb);
    }


    public enum GateOrientation
    {
        Horizontal,
        Vertical,
        Square
    }

    public GateOrientation GetOrientation()
    {
        if (size.x == size.y) return GateOrientation.Square;
        return size.x > size.y ? GateOrientation.Horizontal : GateOrientation.Vertical;
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = gateColor;
        for (int x = 0; x < size.x; x++)
        for (int y = 0; y < size.y; y++)
        {
            Vector3 worldPos = transform.position + new Vector3(x, 0f, y);
            Gizmos.DrawWireCube(worldPos + Vector3.one * 0.5f, Vector3.one * 0.9f);
        }
    }

}
