using Larje.Core.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditorItemPreviewDrawer : MonoBehaviour
{
    [SerializeField] private Sprite _customSprite;
    [SerializeField] private Color _availableColor;
    [SerializeField] private Color _notAvailableColor;

    public static LevelEditorItemPreviewDrawer Instance { get; private set; }


    private void Awake()
    {
        Instance = this;
    }

    public void UpdatePreview(LevelEditorToolArgs args, List<LevelEditorItem> spawnedItems, List<Vector2Int> points) 
    {
        ClearPreview();

        if (args.controllEnabled && args.curentItem)
        {
            foreach (Vector2Int point in points)
            {
                SpriteRenderer itemPreview = new GameObject("Item Preview").AddComponent<SpriteRenderer>();
                itemPreview.transform.SetParent(transform);
                itemPreview.transform.position = new Vector3(point.x, point.y, -10);
                itemPreview.sprite = _customSprite ? _customSprite : args.curentItem.GetPreview();
                itemPreview.color = args.curentItem.CanBePlaced(spawnedItems, point) ? _availableColor : _notAvailableColor;
            }
        }
    }

    public void ClearPreview() 
    {
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }
}
