using System;
using Larje.Core.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditorItemPreviewDrawer : MonoBehaviour
{
    public static LevelEditorItemPreviewDrawer Instance { get; private set; }

    [Serializable]
    public class PreviewDraverOption 
    {
        public Sprite Sprite;
        public Color AvailableColor;
        public Color NotAvailableColor;
    }


    [SerializeField] private PreviewDraverOption _defaultOptions;
    [SerializeField] private PreviewDraverOption _eraserOptions;

    public PreviewDraverOption EraserOptions => _eraserOptions;


    private void Awake()
    {
        Instance = this;
    }

    public void UpdatePreview(LevelEditorToolArgs args, List<LevelEditorItem> spawnedItems, List<Vector2Int> points, PreviewDraverOption customOptions = null) 
    {
        ClearPreview();

        if (args.controllEnabled && args.curentItem)
        {
            foreach (Vector2Int point in points)
            {
                SpriteRenderer itemPreview = new GameObject("Item Preview").AddComponent<SpriteRenderer>();
                itemPreview.transform.SetParent(transform);
                itemPreview.transform.position = new Vector3(point.x, point.y, -10);
                itemPreview.sprite = customOptions != null ? customOptions.Sprite : _defaultOptions.Sprite;

                Color availableColor = customOptions != null ? customOptions.AvailableColor : _defaultOptions.AvailableColor;
                Color notAvailableColor = customOptions != null ? customOptions.NotAvailableColor : _defaultOptions.NotAvailableColor;
                itemPreview.color = args.curentItem.CanBePlaced(spawnedItems, point) ? availableColor : notAvailableColor;
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
