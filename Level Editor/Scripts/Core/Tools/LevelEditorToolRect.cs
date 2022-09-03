using Larje.Core.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditorToolRect : LevelEditorTool
{
    public LevelEditorToolRect(List<LevelEditorItem> instancedItems, GameObject levelHolder) : base(instancedItems, levelHolder) { }

    public override void ComputeTool(LevelEditorToolArgs args)
    {
        Vector2Int position = LarjeUtility.FloorVector2(_camera.ScreenToWorldPoint(Input.mousePosition));
        List<Vector2Int> points = new List<Vector2Int>();



        LevelEditorItemPreviewDrawer.Instance.UpdatePreview(args, _instancedItems, points);
    }
}
