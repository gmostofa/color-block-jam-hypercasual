using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BlockController : MonoBehaviour
{
    public Vector2Int gridPos;
    public Vector2Int size = Vector2Int.one;
    public int blockID; // Unique to match door
    public bool isSolved = false;

    public Color blockColor;

    
    private Vector3 dragStartWorld;
    private Vector2Int originalGridPos;
    private bool isDragging = false;

    void Start()
    {
        ResizeBlock();
        SnapToGrid();
        GridManager.Instance.RegisterBlock(this);
        GameManager.Instance.RegisterBlock(this);
    }

    void OnMouseDown()
    {
        dragStartWorld = GetMouseWorldPosition();
        originalGridPos = gridPos;
        isDragging = true;
        GridManager.Instance.UnregisterBlock(this);
        HighlightManager.Instance.ShowHighlights(GetOccupiedTiles(gridPos), true);
    }

    void OnMouseDrag()
    {
        if (!isDragging || isSolved) return;

        Vector3 currentMouse = GetMouseWorldPosition();
        Vector3 delta = currentMouse - dragStartWorld;

        int moveX = Mathf.RoundToInt(delta.x / GridManager.Instance.cellSize);
        int moveY = Mathf.RoundToInt(delta.z / GridManager.Instance.cellSize);

        Vector2Int tryGridPos = originalGridPos + new Vector2Int(moveX, moveY);
        tryGridPos.x = Mathf.Clamp(tryGridPos.x, 0, GridManager.Instance.width - size.x);
        tryGridPos.y = Mathf.Clamp(tryGridPos.y, 0, GridManager.Instance.height - size.y);

        if (CanMoveTo(tryGridPos))
        {
            gridPos = tryGridPos;
            SnapToGrid();
            HighlightManager.Instance.ShowHighlights(GetOccupiedTiles(tryGridPos), false);
        }
    }

    void OnMouseUp()
    {
        if (isSolved) return;

        isDragging = false;
        GridManager.Instance.RegisterBlock(this);
        HighlightManager.Instance.ClearHighlights();

        // ✅ Only now: check if on correct door
        if (GameManager.Instance.IsOverCorrectGate(this))
        {
            // ✅ Add small delay or feedback before vanish
            Invoke(nameof(SolveAndVanish), 0.1f); // Or use a coroutine if preferred
        }
    }

    void SolveAndVanish()
    {
        isSolved = true;
        GridManager.Instance.UnregisterBlock(this);
        transform.DOKill();

        transform.DOPunchScale(Vector3.one * 0.2f, 0.2f, 8, 0.5f).OnComplete(() =>
        {
            transform.DOScale(Vector3.zero, 0.25f)
                .SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    GameManager.Instance.MarkBlockSolved(this);
                    Destroy(gameObject);
                });
        });
    }


    public bool CanMoveTo(Vector2Int target)
    {
        for (int x = 0; x < size.x; x++)
            for (int y = 0; y < size.y; y++)
            {
                Vector2Int checkPos = target + new Vector2Int(x, y);
                if (!GridManager.Instance.IsInsideGrid(checkPos) ||
                    GridManager.Instance.IsTileOccupied(checkPos, this))
                    return false;
            }
        return true;
    }
    
    public List<Vector2Int> GetOccupiedTiles()
    {
        List<Vector2Int> tiles = new();
        for (int x = 0; x < size.x; x++)
        for (int y = 0; y < size.y; y++)
            tiles.Add(gridPos + new Vector2Int(x, y));
        return tiles;
    }

    public List<Vector2Int> GetOccupiedTiles(Vector2Int atPosition)
    {
        List<Vector2Int> tiles = new();
        for (int x = 0; x < size.x; x++)
        for (int y = 0; y < size.y; y++)
            tiles.Add(atPosition + new Vector2Int(x, y));
        return tiles;
    }


    private void ResizeBlock()
    {
        float cell = GridManager.Instance.cellSize;
        transform.localScale = new Vector3(size.x * cell, 1f, size.y * cell);
    }

    private void SnapToGrid()
    {
        Vector3 baseWorldPos = GridManager.Instance.GetWorldPosition(gridPos.x, gridPos.y);
        Vector3 centerOffset = new Vector3(
            (size.x - 1) * GridManager.Instance.cellSize * 0.5f,
            0f,
            (size.y - 1) * GridManager.Instance.cellSize * 0.5f
        );

        transform.DOMove(baseWorldPos + centerOffset, 0.25f).SetEase(Ease.OutQuad);
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        if (plane.Raycast(ray, out float dist))
            return ray.GetPoint(dist);
        return Vector3.zero;
    }

}
