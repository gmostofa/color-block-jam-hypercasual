using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private List<BlockController> allBlocks = new();
    private List<DoorTile> allDoors = new();

    private void Awake() => Instance = this;

    void Start()
    {
        allDoors.AddRange(FindObjectsOfType<DoorTile>());
    }

    public void RegisterBlock(BlockController block)
    {
        if (!allBlocks.Contains(block))
            allBlocks.Add(block);
    }

    public bool IsOverCorrectGate(BlockController block)
    {
        var blockTiles = block.GetOccupiedTiles(block.gridPos);

        foreach (var gate in allDoors)
        {
            if (!ColorsMatch(block.blockColor, gate.gateColor)) continue;

            var gateTiles = gate.GetOccupiedTiles();
            if (blockTiles.Count != gateTiles.Count) continue;

            bool exactMatch = true;
            foreach (var tile in blockTiles)
            {
                if (!gateTiles.Contains(tile))
                {
                    exactMatch = false;
                    break;
                }
            }

            if (exactMatch)
                return true;
        }

        return false;
    }

    private bool ColorsMatch(Color a, Color b)
    {
        return Mathf.Approximately(a.r, b.r) &&
               Mathf.Approximately(a.g, b.g) &&
               Mathf.Approximately(a.b, b.b);
    }



    public void MarkBlockSolved(BlockController block)
    {
        allBlocks.Remove(block);
        if (allBlocks.Count == 0)
        {
            OnLevelComplete();
        }
    }

    private void OnLevelComplete()
    {
        Debug.Log("🎉 Level Complete!");
        // Show UI, next level, etc.
    }
}