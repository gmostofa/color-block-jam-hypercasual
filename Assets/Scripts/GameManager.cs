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

    public bool IsOverCorrectDoor(BlockController block)
    {
        var blockTiles = block.GetOccupiedTiles(block.gridPos);

        foreach (var door in allDoors)
        {
            if (door.doorID != block.blockID)
                continue;

            var doorTiles = door.GetOccupiedTiles();

            if (blockTiles.Count != doorTiles.Count)
                continue;

            bool match = true;
            for (int i = 0; i < blockTiles.Count; i++)
            {
                if (!doorTiles.Contains(blockTiles[i]))
                {
                    match = false;
                    break;
                }
            }

            if (match)
                return true;
        }

        return false;
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