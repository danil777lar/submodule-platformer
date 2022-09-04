using Larje.Core.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditorItemPreviewDrawer : MonoBehaviour
{
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
                itemPreview.sprite = args.curentItem.GetPreview();
                itemPreview.color = args.curentItem.CanBePlaced(spawnedItems, point) ? Color.green : Color.red;
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
