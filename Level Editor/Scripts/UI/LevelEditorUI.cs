using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Larje.Core.Services.UI;

public class LevelEditorUI : MonoBehaviour
{
    [SerializeField] private LevelEditorItemsDB _itemsDB;

    [Header("Links")]
    [SerializeField] private LevelEditorCamera _levelEditorCamera;

    [Header("Parts")]
    [SerializeField] private RectTransform _root;
    [SerializeField] private RectTransformEvents _workField;
     
    [Header("Inventory")]
    [SerializeField] private RectTransform _inventoryPanel;
    [SerializeField] private LevelEditorInventoryTab _tabPrefab;

    private int _curentLevelEditor = 0;
    private List<LevelEditorGenerator> _levelEditors;

    public RectTransform Root => _root;
    public RectTransform WorkField => (RectTransform)_workField.transform;


    private void Awake()
    {
        _levelEditors = new List<LevelEditorGenerator>();
        _levelEditors.Add(new LevelEditorGenerator());
    }

    private void Start()
    {
        BuildInventoryTabs();
        _workField.PointerEnter += (data) => OnPointerStateChanged(true);
        _workField.PointerExit += (data) => OnPointerStateChanged(false);
    }

    private void Update()
    {
        _levelEditors[_curentLevelEditor].ComputeTool();
    }


    public void SetCurentItem(LevelEditorItem item) 
    {
        _levelEditors[_curentLevelEditor].CurrentItem = item;
    }

    public void SetCurentTool(LevelEditorGenerator.LevelEditorToolType tool)
    {
        _levelEditors[_curentLevelEditor].CurrentTool = tool;
    }

    public LevelEditorGenerator.LevelEditorToolType GetCurentTool() 
    {
        return _levelEditors[_curentLevelEditor].CurrentTool;
    }


    private void OnPointerStateChanged(bool arg) 
    {
        _levelEditors[_curentLevelEditor].ControllEnabled = arg;
        _levelEditorCamera.SetActiveControll(arg);
    }

    private void BuildInventoryTabs() 
    {
        foreach (LevelEditorItemsDB.ItemGroup itemGroup in _itemsDB.ItemGroups)
        {
            Instantiate(_tabPrefab, _inventoryPanel).Build(itemGroup, this);
        }
    }
}
