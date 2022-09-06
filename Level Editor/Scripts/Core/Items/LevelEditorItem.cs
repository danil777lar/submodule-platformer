using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LevelEditorItem : MonoBehaviour
{
    [SerializeField] protected int _levelLayer;
    [SerializeField] protected Vector2Int _size;
    [SerializeField] protected ItemAvailableTools _availableTools;

    public int LevelLayer => _levelLayer;
    public Vector2Int Position { get; protected set; }
    public ItemAvailableTools AvailableTools => _availableTools;



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

    public bool HasIntersectionWith(List<Vector2Int> cells) 
    {
        List<Vector2Int> selfCells = GetEngagedCells(Position);
        foreach (Vector2Int cell in cells) 
        {
            if (selfCells.Contains(cell))
            {
                return true;
            }
        }
        return false;
    }


    virtual public void Place(List<LevelEditorItem> spawnedItems, Vector2Int position) 
    {
        Position = position;
        transform.position = new Vector3(position.x, position.y, -_levelLayer);
        spawnedItems.Add(this);
    }
    virtual public void Remove(List<LevelEditorItem> spawnedItems) 
    {
        spawnedItems.Remove(this);
        Destroy(gameObject);
    }

    abstract public bool CanBePlaced(List<LevelEditorItem> spawnedItems, Vector2Int position);
    abstract public Sprite GetPreview();
}
