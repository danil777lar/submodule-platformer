using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/LevelEditor/ItemsDB")]
public class LevelEditorItemsDB : ScriptableObject
{
    [Serializable]
    public class ItemGroup 
    {
        public string GroupName;
        public Sprite GroupIcon;
        public List<LevelEditorItem> Items;
    }

    public List<ItemGroup> ItemGroups;
}
