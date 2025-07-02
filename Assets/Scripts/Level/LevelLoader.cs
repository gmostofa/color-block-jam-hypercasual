using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public GameObject blockPrefab;

    void Start()
    {
        SpawnBlock(new Vector2Int(1, 1), new Vector2Int(1, 2)); // 1x2 vertical block
        SpawnBlock(new Vector2Int(1, 4), new Vector2Int(2, 1)); // 2x1 horizontal block
    }

    void SpawnBlock(Vector2Int gridPos, Vector2Int size)
    {
        GameObject block = Instantiate(blockPrefab);
        BlockController ctrl = block.GetComponent<BlockController>();
        ctrl.gridPos = gridPos;
        ctrl.size = size;
    }
}