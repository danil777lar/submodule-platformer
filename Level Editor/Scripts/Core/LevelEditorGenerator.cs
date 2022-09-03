using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Larje.Core.Utility;

public class LevelEditorGenerator
{
    public enum LevelEditorToolType 
    {
        Brush, 
        Eraser,
        Line,
        Rect, 
        FilledRect
    }

    public bool ControllEnabled;
    public LevelEditorItem CurrentItem;
    public LevelEditorToolType CurrentTool;

    private GameObject _levelHolder;
    private List<LevelEditorItem> _instancedItems;
    private Dictionary<LevelEditorToolType, LevelEditorTool> _tools;


    public LevelEditorGenerator()
    {
        _levelHolder = new GameObject("New Level");

        _tools = new Dictionary<LevelEditorToolType, LevelEditorTool>();
        _tools.Add(LevelEditorToolType.Brush, new LevelEditorToolBrush(_instancedItems, _levelHolder));
        _tools.Add(LevelEditorToolType.Eraser, new LevelEditorToolEraser(_instancedItems, _levelHolder));
        _tools.Add(LevelEditorToolType.Line, new LevelEditorToolLine(_instancedItems, _levelHolder));
        _tools.Add(LevelEditorToolType.Rect, new LevelEditorToolRect(_instancedItems, _levelHolder));
        _tools.Add(LevelEditorToolType.FilledRect, new LevelEditorToolFilledRect(_instancedItems, _levelHolder));
    }

    public void ComputeTool()
    {
        _tools[CurrentTool].ComputeTool(CurrentItem, ControllEnabled);
    }
}