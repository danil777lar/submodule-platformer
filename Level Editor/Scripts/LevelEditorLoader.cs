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

    public LevelEditorItemsDB ItemDB => _itemDB;


    public override void Init()
    {
        
    }

    public void SaveLevel(List<LevelEditorItem> instancedItems) 
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
        string filePath = $"{Application.persistentDataPath}/level.json";
        File.WriteAllText(filePath, jsonText);
    }

    public void LoadLevel(out GameObject levelRoot, out List<LevelEditorItem> levelItems) 
    {
        levelRoot = new GameObject("level");
        levelItems = new List<LevelEditorItem>();

        string filePath = $"{Application.persistentDataPath}/level.json";
        string jsonText = File.ReadAllText(filePath);

        List<ItemSerializationBridge> serializeBridges = JsonConvert.DeserializeObject<List<ItemSerializationBridge>>(jsonText);
        foreach (ItemSerializationBridge bridge in serializeBridges) 
        {
            Vector2Int position = new Vector2Int(Int32.Parse(bridge.PositionX), Int32.Parse(bridge.PositionY));
            LevelEditorItem itemInstance = Instantiate(_itemDB.GetItemByPath(bridge.ItemPath), levelRoot.transform);
            itemInstance.Place(levelItems, position);
        }
    }


    private class ItemSerializationBridge 
    {
        [JsonProperty("path")] public string ItemPath;
        [JsonProperty("position_x")] public string PositionX;
        [JsonProperty("position_y")] public string PositionY;
    }
}
