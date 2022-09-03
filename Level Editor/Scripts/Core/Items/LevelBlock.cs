using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBlock : LevelEditorItem
{
    [SerializeField] private List<Sprite> _mainSprites;
    [Header("Side Covers")]
    [SerializeField] GameObject _topCover;
    [SerializeField] GameObject _rightCover;
    [SerializeField] GameObject _bottomCover;
    [SerializeField] GameObject _leftCover;



    public override void Place(List<LevelEditorItem> spawnedItems, Vector2Int position)
    {
        Position = position;
        transform.position = (Vector2)position;
        spawnedItems.Add(this);

        RecalculateSideCovers(spawnedItems);
        ((LevelBlock)spawnedItems.Find((item) => (item is LevelBlock) && item.Position == position + Vector2Int.up))?.RecalculateSideCovers(spawnedItems);
        ((LevelBlock)spawnedItems.Find((item) => (item is LevelBlock) && item.Position == position + Vector2Int.right))?.RecalculateSideCovers(spawnedItems);
        ((LevelBlock)spawnedItems.Find((item) => (item is LevelBlock) && item.Position == position + Vector2Int.down))?.RecalculateSideCovers(spawnedItems);
        ((LevelBlock)spawnedItems.Find((item) => (item is LevelBlock) && item.Position == position + Vector2Int.left))?.RecalculateSideCovers(spawnedItems);
    }

    public override bool CanBePlaced(List<LevelEditorItem> spawnedItems, Vector2Int position)
    {
        foreach (LevelEditorItem item in spawnedItems) 
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
        _topCover.gameObject.SetActive(!spawnedItems.Find((item) => (item is LevelBlock) && item.Position == Position + Vector2Int.up));
        _rightCover.gameObject.SetActive(!spawnedItems.Find((item) => (item is LevelBlock) && item.Position == Position + Vector2Int.right));
        _bottomCover.gameObject.SetActive(!spawnedItems.Find((item) => (item is LevelBlock) && item.Position == Position + Vector2Int.down));
        _leftCover.gameObject.SetActive(!spawnedItems.Find((item) => (item is LevelBlock) && item.Position == Position + Vector2Int.left));
    }
}
