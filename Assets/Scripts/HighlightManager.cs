using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HighlightManager : MonoBehaviour
{
    public static HighlightManager Instance;
    public GameObject highlightPrefab;
    private List<GameObject> activeHighlights = new();
    private Queue<GameObject> pool = new();

    private void Awake() => Instance = this;

    public void ShowHighlights(List<Vector2Int> tiles, bool isInstant = false)
    {
        // Disable extra highlights
        while (activeHighlights.Count > tiles.Count)
        {
            var last = activeHighlights[^1];
            last.SetActive(false);
            pool.Enqueue(last);
            activeHighlights.RemoveAt(activeHighlights.Count - 1);
        }

        for (int i = 0; i < tiles.Count; i++)
        {
            Vector2Int tile = tiles[i];
            if (!GridManager.Instance.IsInsideGrid(tile)) continue;

            Vector3 worldPos = GridManager.Instance.GetWorldPosition(tile.x, tile.y) + new Vector3(0, 0.01f, 0);

            GameObject highlight;
            if (i < activeHighlights.Count)
            {
                highlight = activeHighlights[i];
                highlight.SetActive(true);
            }
            else
            {
                highlight = pool.Count > 0 ? pool.Dequeue() : Instantiate(highlightPrefab, transform);
                activeHighlights.Add(highlight);
            }

            highlight.transform.DOKill(); // Stop any previous animation

            if (isInstant)
            {
                highlight.transform.position = worldPos;
            }
            else
            {
                highlight.transform.DOMove(worldPos, 0.15f).SetEase(Ease.OutQuad);
            }
        }
    }


    public void ClearHighlights()
    {
        foreach (var highlight in activeHighlights)
        {
            highlight.SetActive(false);
            pool.Enqueue(highlight);
        }
        activeHighlights.Clear();
    }
}