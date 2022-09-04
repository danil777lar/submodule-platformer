using Larje.Core.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditorToolLine : LevelEditorTool
{
    private bool _pointerDown;
    private Vector2Int _pointerDownPosition;


    public LevelEditorToolLine(List<LevelEditorItem> instancedItems, GameObject levelHolder) : base(instancedItems, levelHolder) { }

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
        Vector2Int direction = position - _pointerDownPosition;
        int stepCount = Mathf.Max(Mathf.Abs(direction.x), Mathf.Abs(direction.y));
        for (int i = 0; i <= stepCount; i++)
        {
            Vector2Int point = Vector2Int.RoundToInt(Vector2.Lerp(_pointerDownPosition, position, (float)i / (float)stepCount));
            if (!points.Contains(point))
            {
                points.Add(point);
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
