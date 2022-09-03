using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LevelEditorTool
{
    protected GameObject _levelHolder;
    protected List<LevelEditorItem> _instancedItems;

    public LevelEditorTool(List<LevelEditorItem> instancedItems, GameObject levelHolder) 
    {
        _levelHolder = levelHolder;
        _instancedItems = instancedItems;
    }


    public abstract void ComputeTool(LevelEditorItem curentItem, bool controllEnabled);
}
