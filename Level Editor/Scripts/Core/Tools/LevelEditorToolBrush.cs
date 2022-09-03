using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditorToolBrush : LevelEditorTool
{
    public LevelEditorToolBrush(List<LevelEditorItem> instancedItems, GameObject levelHolder) : base(instancedItems, levelHolder) {}

    public override void ComputeTool(LevelEditorItem curentItem, bool controllEnabled)
    {
    }


    private void UpdateItemPreview()
    {
/*        _itemPreview.gameObject.SetActive(_controllEnabled && _currentItem);

        if (_controllEnabled && _currentItem)
        {
            _itemPreview.sprite = _currentItem.GetPreview();
            _itemPreview.color = _currentItem.CanBePlaced() ? Color.green : Color.red;
            _itemPreview.transform.position = (Vector2)LarjeUtility.FloorVector2(_camera.ScreenToWorldPoint(Input.mousePosition));
        }*/
    }
}
