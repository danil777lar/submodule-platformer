using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBlock : LevelEditorItem
{
    [SerializeField] private List<Sprite> _mainSprites;



    public override bool CanBePlaced()
    {
        return true;
    }

    public override Sprite GetPreview()
    {
        return _mainSprites[0];
    }

    public override void Place()
    {
        
    }
}
