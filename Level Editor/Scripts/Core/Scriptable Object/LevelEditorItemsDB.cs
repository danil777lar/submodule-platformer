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


    public string GetItemPath(LevelEditorItem item) 
    {
        string targetName = item.gameObject.name;
        if (targetName.Contains("(Clone)")) 
        {
            targetName = targetName.Replace("(Clone)", "");
        }
        Debug.Log(targetName + "  " + targetName.Contains("(Clone)"));
        ItemGroup group = ItemGroups.Find((group) => group.Items.Find((i) => i.gameObject.name == targetName));
        if (group == null) 
        {
            return null;
        }
        return $"{group.GroupName}/{targetName}";
    }

    public LevelEditorItem GetItemByPath(string path) 
    {
        string groupName = path.Split('/')[0];
        string prefabName = path.Split('/')[1];

        ItemGroup itemGroup = ItemGroups.Find((group) => group.GroupName == groupName);
        return itemGroup.Items.Find((item) => item.gameObject.name == prefabName);
    }
}
