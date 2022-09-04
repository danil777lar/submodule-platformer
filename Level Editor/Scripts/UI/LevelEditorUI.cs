using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Larje.Core.Services.UI;
using Larje.Core.Services;

public class LevelEditorUI : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] private LevelEditorCamera _levelEditorCamera;

    [Header("Parts")]
    [SerializeField] private RectTransform _root;
    [SerializeField] private RectTransformEvents _workField;
     
    [Header("Inventory")]
    [SerializeField] private RectTransform _inventoryPanel;
    [SerializeField] private LevelEditorInventoryTab _tabPrefab;

    [Header("Toolbar Buttons")]
    [SerializeField] private Button _buttonSave;
    [SerializeField] private Button _buttonLoad;

    private int _curentLevelEditor = 0;
    private LevelEditorLoader _levelLoader;
    private List<LevelEditorGenerator> _levelEditors;

    public RectTransform Root => _root;
    public RectTransform WorkField => (RectTransform)_workField.transform;

    public Action CurentItemChanged;


    private void Awake()
    {
        _levelEditors = new List<LevelEditorGenerator>();
        _levelEditors.Add(new LevelEditorGenerator());
        _levelEditors[_curentLevelEditor].Enabled = true;
    }

    private void Start()
    {
        _levelLoader = ServiceLocator.Default.GetService<LevelEditorLoader>();

        BuildInventoryTabs();
        _workField.PointerEnter += (data) => OnPointerStateChanged(true);
        _workField.PointerExit += (data) => OnPointerStateChanged(false);

        _buttonSave.onClick.AddListener(() => 
        {
            _levelLoader.SaveLevel(_levelEditors[_curentLevelEditor].InstancedItems);
        });

        _buttonLoad.onClick.AddListener(() => 
        {
            _levelEditors[_curentLevelEditor].Enabled = false;
            _levelEditors[_curentLevelEditor].LevelHolder.SetActive(false);

            _levelLoader.LoadLevel(out GameObject levelHolder, out List<LevelEditorItem> levelItems);
            _levelEditors.Add(new LevelEditorGenerator(levelHolder, levelItems));
            _levelEditors[_curentLevelEditor].Enabled = true;
            _curentLevelEditor++;
        });
    }

    private void Update()
    {
        _levelEditors[_curentLevelEditor].ComputeTool();
    }


    public void SetCurentItem(LevelEditorItem item) 
    {
        _levelEditors[_curentLevelEditor].CurrentItem = item;
        CurentItemChanged?.Invoke();
    }

    public void SetCurentTool(LevelEditorToolType tool)
    {
        _levelEditors[_curentLevelEditor].CurrentTool = tool;
    }

    public void SetThicknes(int thicknes) 
    {
        _levelEditors[_curentLevelEditor].Thicknes = thicknes;
    }

    public LevelEditorToolType GetCurentTool() 
    {
        return _levelEditors[_curentLevelEditor].CurrentTool;
    }

    public LevelEditorItem GetCurentItem() 
    {
        return _levelEditors[_curentLevelEditor].CurrentItem;
    }


    private void OnPointerStateChanged(bool arg) 
    {
        _levelEditors[_curentLevelEditor].ControllEnabled = arg;
        _levelEditorCamera.SetActiveControll(arg);
    }

    private void BuildInventoryTabs() 
    {
        foreach (LevelEditorItemsDB.ItemGroup itemGroup in _levelLoader.ItemDB.ItemGroups)
        {
            Instantiate(_tabPrefab, _inventoryPanel).Build(itemGroup, this);
        }
    }
}
