using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LevelEditorItem : MonoBehaviour
{
    [SerializeField] protected Vector2Int _size;
    public Vector2Int Position { get; protected set; }


    protected void OnValidate()
    {
        _size.x = Mathf.Max(_size.x, 1);
        _size.y = Mathf.Max(_size.y, 1);
    }


    public List<Vector2Int> GetEngagedCells(Vector2Int position) 
    {
        List<Vector2Int> result = new List<Vector2Int>();
        for (int dx = 0; dx < _size.x; dx++) 
        {
            for (int dy = 0; dy < _size.y; dy++)
            {
                result.Add(position + new Vector2Int(dx, dy));
            }
        }
        return result;
    }


    abstract public void Place(List<LevelEditorItem> spawnedItems, Vector2Int position);
    abstract public bool CanBePlaced(List<LevelEditorItem> spawnedItems, Vector2Int position);
    abstract public Sprite GetPreview();
}
