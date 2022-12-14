using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using Larje.Core.Services;

[BindService(typeof(LevelEditorLoader))]
public class LevelEditorLoader : Service
{
    [SerializeField] private LevelEditorItemsDB _itemDB;
    [SerializeField] private string _localLevelsFolder;

    public LevelEditorItemsDB ItemDB => _itemDB;


    public override void Init()
    {
        string fullPah = $"{Application.persistentDataPath}/{_localLevelsFolder}";
        if (!Directory.Exists(fullPah)) 
        {
            Directory.CreateDirectory(fullPah);
        }
    }

    public void SaveLevel(string path, List<LevelEditorItem> instancedItems) 
    {
        List<ItemSerializationBridge> serializeBridges = new List<ItemSerializationBridge>();
        foreach (LevelEditorItem item in instancedItems) 
        {
            ItemSerializationBridge bridge = new ItemSerializationBridge();
            bridge.ItemPath = _itemDB.GetItemPath(item);
            bridge.PositionX = item.Position.x.ToString();
            bridge.PositionY = item.Position.y.ToString();
            serializeBridges.Add(bridge);
        }

        string jsonText = JsonConvert.SerializeObject(serializeBridges);
        File.WriteAllText(path, jsonText);
    }

    public void LoadLevel(string path, out GameObject levelRoot, out List<LevelEditorItem> levelItems) 
    {
        levelRoot = new GameObject("level");
        levelItems = new List<LevelEditorItem>();
        string jsonText = File.ReadAllText(path);

        List<ItemSerializationBridge> serializeBridges = JsonConvert.DeserializeObject<List<ItemSerializationBridge>>(jsonText);
        foreach (ItemSerializationBridge bridge in serializeBridges) 
        {
            Vector2Int position = new Vector2Int(Int32.Parse(bridge.PositionX), Int32.Parse(bridge.PositionY));
            LevelEditorItem itemInstance = Instantiate(_itemDB.GetItemByPath(bridge.ItemPath), levelRoot.transform);
            itemInstance.Place(levelItems, position);
        }
    }

    public List<string> GetLocalLevelsList() 
    {
        List<string> levels = new List<string>(Directory.GetFiles($"{Application.persistentDataPath}/{_localLevelsFolder}"));
        for (int i = 0; i < levels.Count; i++) 
        {
            levels[i] = levels[i];
        }
        return levels;
    }


    private class ItemSerializationBridge 
    {
        [JsonProperty("path")] public string ItemPath;
        [JsonProperty("position_x")] public string PositionX;
        [JsonProperty("position_y")] public string PositionY;
    }
}
