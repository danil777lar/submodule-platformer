using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditorToolEraser : LevelEditorTool
{
    public LevelEditorToolEraser(List<LevelEditorItem> instancedItems, GameObject levelHolder) : base(instancedItems, levelHolder) { }

    public override void ComputeTool(LevelEditorItem curentItem, bool controllEnabled)
    {
    }
}
