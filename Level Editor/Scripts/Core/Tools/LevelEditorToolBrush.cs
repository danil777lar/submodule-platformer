using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Larje.Core.Utility;

public class LevelEditorToolBrush : LevelEditorTool
{
    public LevelEditorToolBrush(List<LevelEditorItem> instancedItems, GameObject levelHolder) : base(instancedItems, levelHolder) { }

    public override void ComputeTool(LevelEditorToolArgs args)
    {
        Vector2Int position = LarjeUtility.FloorVector2(_camera.ScreenToWorldPoint(Input.mousePosition));
        List<Vector2Int> points = LarjeUtility.GetPixelsInRange(position, args.thicknes);

        if (Input.GetMouseButton(0) && args.controllEnabled && args.curentItem) 
        {
            SpawnItems(args, points);
        }

        LevelEditorItemPreviewDrawer.Instance.UpdatePreview(args, _instancedItems, points);
    }

    private void SpawnItems(LevelEditorToolArgs args, List<Vector2Int> points) 
    {
        foreach (Vector2Int point in points) 
        {
            if (args.curentItem.CanBePlaced(_instancedItems, point)) 
            {
                LevelEditorItem instance = GameObject.Instantiate(args.curentItem);
                instance.Place(_instancedItems, point);
                instance.transform.SetParent(_levelHolder.transform);
            }
        }
    }
}
