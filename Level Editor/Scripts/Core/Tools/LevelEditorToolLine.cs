using Larje.Core.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditorToolLine : LevelEditorTool
{
    private Vector2Int _pointerDownPosition;


    public LevelEditorToolLine(List<LevelEditorItem> instancedItems, GameObject levelHolder) : base(instancedItems, levelHolder) { }

    public override void ComputeTool(LevelEditorToolArgs args)
    {
        Vector2Int position = LarjeUtility.FloorVector2(_camera.ScreenToWorldPoint(Input.mousePosition));
        List<Vector2Int> points = new List<Vector2Int>();
        


        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) 
        {
            _pointerDownPosition = position;
        }

        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            points.Add(_pointerDownPosition);
            points.Add(position);
        }
        else 
        {
            points.Add(position);
        }



        LevelEditorItemPreviewDrawer.Instance.UpdatePreview(args, _instancedItems, points);
    }
}
