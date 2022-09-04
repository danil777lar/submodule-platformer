using Larje.Core.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditorToolFilledRect : LevelEditorTool
{
    private bool _pointerDown;
    private Vector2Int _pointerDownPosition;

    public LevelEditorToolFilledRect(List<LevelEditorItem> instancedItems, GameObject levelHolder) : base(instancedItems, levelHolder) { }


    public override void ComputeTool(LevelEditorToolArgs args)
    {
        if (args.controllEnabled)
        {
            Vector2Int position = LarjeUtility.FloorVector2(_camera.ScreenToWorldPoint(Input.mousePosition));
            List<Vector2Int> points = new List<Vector2Int>();

            if (Input.GetMouseButtonDown(0))
            {
                _pointerDown = true;
                _pointerDownPosition = position;
            }

            if (Input.GetMouseButton(0) && _pointerDown)
            {
                points = ComputePoints(args, position);
            }
            else
            {
                points = LarjeUtility.GetPixelsInRange(position, args.thicknes);
            }
            LevelEditorItemPreviewDrawer.Instance.UpdatePreview(args, _instancedItems, points);

            if (Input.GetMouseButtonUp(0) && _pointerDown)
            {
                _pointerDown = false;
                points = ComputePoints(args, position);
                SpawnItems(args, points);
            }
        }
        else
        {
            _pointerDown = false;
            LevelEditorItemPreviewDrawer.Instance.ClearPreview();
        }
    }

    private List<Vector2Int> ComputePoints(LevelEditorToolArgs args, Vector2Int position)
    {
        List<Vector2Int> points = new List<Vector2Int>();
        points.Add(_pointerDownPosition);
        points.Add(position);

        for (int x = Mathf.Min(_pointerDownPosition.x, position.x); x <= Mathf.Max(_pointerDownPosition.x, position.x); x++)
        {
            for (int y = Mathf.Min(_pointerDownPosition.y, position.y); y <= Mathf.Max(_pointerDownPosition.y, position.y); y++)
            {
                Vector2Int point = new Vector2Int(x, y);
                if (!points.Contains(point)) 
                {
                    points.Add(point);
                }
            }
        }
        


        List<Vector2Int> radiusPoints = new List<Vector2Int>();
        foreach (Vector2Int point in points)
        {
            foreach (Vector2Int radiusPoint in LarjeUtility.GetPixelsInRange(point, args.thicknes))
            {
                if (!radiusPoints.Contains(radiusPoint))
                {
                    radiusPoints.Add(radiusPoint);
                }
            }
        }
        foreach (Vector2Int radiusPoint in radiusPoints)
        {
            if (!points.Contains(radiusPoint))
            {
                points.Add(radiusPoint);
            }
        }
        return points;
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
