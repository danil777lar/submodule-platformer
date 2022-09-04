using Larje.Core.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditorToolEraser : LevelEditorTool
{
    public LevelEditorToolEraser(List<LevelEditorItem> instancedItems, GameObject levelHolder) : base(instancedItems, levelHolder) { }

    public override void ComputeTool(LevelEditorToolArgs args)
    {
        Vector2Int position = LarjeUtility.FloorVector2(_camera.ScreenToWorldPoint(Input.mousePosition));
        List<Vector2Int> points = LarjeUtility.GetPixelsInRange(position, args.thicknes);

        if (Input.GetMouseButton(0))
        {
            foreach (Vector2Int point in points)
            {
                foreach (LevelEditorItem item in _instancedItems.FindAll((item) => item.GetEngagedCells(item.Position).Contains(point)))
                {
                    if (item.LevelLayer == args.curentItem.LevelLayer)
                    {
                        item.Remove(_instancedItems);
                    }
                }
            }
        }

        LevelEditorItemPreviewDrawer.Instance.ClearPreview();
    }
}
