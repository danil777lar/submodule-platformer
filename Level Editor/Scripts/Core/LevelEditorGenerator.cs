using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Larje.Core.Utility;

public class LevelEditorGenerator
{
    public bool ControllEnabled;
    public bool Enabled;
    public int Thicknes = 1;
    public LevelEditorItem CurrentItem;
    public LevelEditorToolType CurrentTool = LevelEditorToolType.Brush;

    private GameObject _levelHolder;
    private List<LevelEditorItem> _instancedItems;
    private Dictionary<LevelEditorToolType, LevelEditorTool> _tools;

    public GameObject LevelHolder => _levelHolder;
    public List<LevelEditorItem> InstancedItems => _instancedItems;


    public LevelEditorGenerator()
    {
        _levelHolder = new GameObject("New Level");
        _instancedItems = new List<LevelEditorItem>();

        InitTools();
    }

    public LevelEditorGenerator(GameObject levelHolder, List<LevelEditorItem> items) 
    {
        _levelHolder = levelHolder;
        _instancedItems = items;

        InitTools();
    }


    public void ComputeTool()
    {
        if (Enabled)
        {
            LevelEditorToolArgs args;
            args.controllEnabled = ControllEnabled;
            args.curentItem = CurrentItem;
            args.thicknes = Thicknes;

            _tools[CurrentTool].ComputeTool(args);
        }
    }


    private void InitTools() 
    {
        _tools = new Dictionary<LevelEditorToolType, LevelEditorTool>();
        _tools.Add(LevelEditorToolType.Brush, new LevelEditorToolBrush(_instancedItems, _levelHolder));
        _tools.Add(LevelEditorToolType.Eraser, new LevelEditorToolEraser(_instancedItems, _levelHolder));
        _tools.Add(LevelEditorToolType.Line, new LevelEditorToolLine(_instancedItems, _levelHolder));
        _tools.Add(LevelEditorToolType.Rect, new LevelEditorToolRect(_instancedItems, _levelHolder));
        _tools.Add(LevelEditorToolType.FilledRect, new LevelEditorToolFilledRect(_instancedItems, _levelHolder));
    }
}
