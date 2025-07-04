using UnityEngine;
using System.Collections.Generic;

public class LevelLoader : MonoBehaviour
{
    private static readonly int Color1 = Shader.PropertyToID("_Color");
    public GameObject blockPrefab;
    public GameObject doorPrefab;

    public List<Color> availableColors;

    void Start()
    {
        // Example: 2 blocks and matching doors
        SpawnPair(new Vector2Int(0, 3), new Vector2Int(1, 0), new Vector2Int(1, 2), 0); // vertical red
        SpawnPair(new Vector2Int(1, 2), new Vector2Int(2, 0), new Vector2Int(2, 1), 1); // horizontal green
    }

    void SpawnPair(Vector2Int blockPos, Vector2Int doorPos, Vector2Int size, int colorIndex)
    {
        Color color = availableColors[colorIndex % availableColors.Count];

        // Spawn Door
        GameObject door = Instantiate(doorPrefab);
        DoorTile doorTile = door.GetComponent<DoorTile>();
        doorTile.gridPosition = doorPos;
        doorTile.size = size;
        doorTile.gateColor = color;
        ApplyColor(door, color);

        // Spawn Block
        GameObject block = Instantiate(blockPrefab);
        BlockController ctrl = block.GetComponent<BlockController>();
        ctrl.gridPos = blockPos;
        ctrl.size = size;
        ctrl.blockColor = color;
        ApplyColor(block, color);
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
}