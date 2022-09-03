using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LevelEditorTool
{
    protected Camera _camera;
    protected GameObject _levelHolder;
    protected List<LevelEditorItem> _instancedItems;

    public LevelEditorTool(List<LevelEditorItem> instancedItems, GameObject levelHolder) 
    {
        _levelHolder = levelHolder;
        _instancedItems = instancedItems;
        _camera = Camera.main;
    }


    public abstract void ComputeTool(LevelEditorToolArgs args);
}

public struct LevelEditorToolArgs 
{
    public bool controllEnabled;
    public int thicknes;
    public LevelEditorItem curentItem;
}