using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LevelEditorToolType
{
    Brush = 1,
    Eraser = 2,
    Line = 4,
    Rect = 8,
    FilledRect = 16
}

[Flags]
public enum ItemAvailableTools
{
    Brush = LevelEditorToolType.Brush,
    Eraser = LevelEditorToolType.Eraser,
    Line = LevelEditorToolType.Line,
    Rect = LevelEditorToolType.Rect,
    FilledRect = LevelEditorToolType.FilledRect
}
