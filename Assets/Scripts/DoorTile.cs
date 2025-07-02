using System.Collections.Generic;
using UnityEngine;

public class DoorTile : MonoBehaviour
{
    public Vector2Int gridPosition;
    public Vector2Int size = Vector2Int.one;
    public int doorID;

    public List<Vector2Int> GetOccupiedTiles()
    {
        List<Vector2Int> tiles = new();
        for (int x = 0; x < size.x; x++)
        for (int y = 0; y < size.y; y++)
            tiles.Add(gridPosition + new Vector2Int(x, y));
        return tiles;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        GridManager grid = FindObjectOfType<GridManager>();
        if (!grid) return;

        Gizmos.color = Color.red;
        foreach (var pos in GetOccupiedTiles())
        {
            Vector3 worldPos = grid.GetWorldPosition(pos.x, pos.y);
            Gizmos.DrawWireCube(worldPos + Vector3.up * 0.1f, Vector3.one * 0.9f);
        }
    }
#endif
}