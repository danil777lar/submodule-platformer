using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBlock : LevelEditorItem
{
    [SerializeField] private bool _isWall;
    [SerializeField] private SpriteRenderer _mainSpriteRenderer;
    [SerializeField] private List<Sprite> _mainSprites;
    [Header("Side Covers")]
    [SerializeField] GameObject _topCover;
    [SerializeField] GameObject _rightCover;
    [SerializeField] GameObject _bottomCover;
    [SerializeField] GameObject _leftCover;

    public bool IsWall => _isWall;


    public override void Place(List<LevelEditorItem> spawnedItems, Vector2Int position)
    {
        base.Place(spawnedItems, position);

        _mainSpriteRenderer.sprite = _mainSprites[UnityEngine.Random.Range(0, _mainSprites.Count)];
        RecalculateSideCovers(spawnedItems);
        ((LevelBlock)spawnedItems.Find((item) => IsNeighbourBlock(item, Vector2Int.up)))?.RecalculateSideCovers(spawnedItems);
        ((LevelBlock)spawnedItems.Find((item) => IsNeighbourBlock(item, Vector2Int.right)))?.RecalculateSideCovers(spawnedItems);
        ((LevelBlock)spawnedItems.Find((item) => IsNeighbourBlock(item, Vector2Int.down)))?.RecalculateSideCovers(spawnedItems);
        ((LevelBlock)spawnedItems.Find((item) => IsNeighbourBlock(item, Vector2Int.left)))?.RecalculateSideCovers(spawnedItems);
    }

    public override void Remove(List<LevelEditorItem> spawnedItems)
    {
        base.Remove(spawnedItems);

        ((LevelBlock)spawnedItems.Find((item) => IsNeighbourBlock(item, Vector2Int.up)))?.RecalculateSideCovers(spawnedItems);
        ((LevelBlock)spawnedItems.Find((item) => IsNeighbourBlock(item, Vector2Int.right)))?.RecalculateSideCovers(spawnedItems);
        ((LevelBlock)spawnedItems.Find((item) => IsNeighbourBlock(item, Vector2Int.down)))?.RecalculateSideCovers(spawnedItems);
        ((LevelBlock)spawnedItems.Find((item) => IsNeighbourBlock(item, Vector2Int.left)))?.RecalculateSideCovers(spawnedItems);
    }

    public override bool CanBePlaced(List<LevelEditorItem> spawnedItems, Vector2Int position)
    {
        foreach (LevelEditorItem item in spawnedItems.FindAll((item) => item.LevelLayer == _levelLayer)) 
        {
            List<Vector2Int> itemCells = item.GetEngagedCells(item.Position);
            foreach (Vector2Int selfPoint in GetEngagedCells(position)) 
            {
                if (itemCells.Contains(selfPoint))
                    return false;
            }
        }

        return true;
    }

    public override Sprite GetPreview()
    {
        return _mainSprites[0];
    }

    public void RecalculateSideCovers(List<LevelEditorItem> spawnedItems) 
    {
        _topCover.gameObject.SetActive(!spawnedItems.Find((item) => IsNeighbourBlock(item, Vector2Int.up)));
        _rightCover.gameObject.SetActive(!spawnedItems.Find((item) => IsNeighbourBlock(item, Vector2Int.right)));
        _bottomCover.gameObject.SetActive(!spawnedItems.Find((item) => IsNeighbourBlock(item, Vector2Int.down)));
        _leftCover.gameObject.SetActive(!spawnedItems.Find((item) => IsNeighbourBlock(item, Vector2Int.left)));
    }

    private bool IsNeighbourBlock(LevelEditorItem item, Vector2Int direction) 
    {
        return (item is LevelBlock) && ((LevelBlock)item).IsWall == _isWall && item.Position == Position + direction;
    }
}
