using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEditor;
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

    [Header("File Tabs")]
    [SerializeField] private LevelEditorFileTab _fileTabPrefab;
    [SerializeField] private RectTransform _fileTabsHolder;

    private LevelEditorLoader _levelLoader;
    private LevelEditorGenerator _curentLevelEditor;
    private List<LevelEditorGenerator> _levelEditors;

    public RectTransform Root => _root;
    public RectTransform WorkField => (RectTransform)_workField.transform;
    public LevelEditorGenerator CurentLevel => _curentLevelEditor;

    public Action CurentItemChanged;


    private void Awake()
    {
        _levelEditors = new List<LevelEditorGenerator>();
        CreateNewLevel();
    }

    private void Start()
    {
        _levelLoader = ServiceLocator.Default.GetService<LevelEditorLoader>();

        BuildInventoryTabs();
        _workField.PointerEnter += (data) => OnPointerStateChanged(true);
        _workField.PointerExit += (data) => OnPointerStateChanged(false);

        _buttonSave.onClick.AddListener(SaveLevel);
        _buttonLoad.onClick.AddListener(OpenLevel);
    }

    private void Update()
    {
        _curentLevelEditor.ComputeTool();
    }


    public void ChooseLevel(LevelEditorGenerator level)
    {
        if (level != _curentLevelEditor)
        {
            _curentLevelEditor.Enabled = false;
            _curentLevelEditor.LevelHolder.SetActive(false);

            _curentLevelEditor = level;
            _curentLevelEditor.Enabled = true;
            _curentLevelEditor.LevelHolder.SetActive(true);
        }
    }

    public void SetCurentItem(LevelEditorItem item)
    {
        _curentLevelEditor.CurrentItem = item;
        CurentItemChanged?.Invoke();
    }

    public void SetCurentTool(LevelEditorToolType tool)
    {
        _curentLevelEditor.CurrentTool = tool;
    }

    public void SetThicknes(int thicknes)
    {
        _curentLevelEditor.Thicknes = thicknes;
    }

    public bool CloseLevel(LevelEditorGenerator level)
    {
        if (_levelEditors.Count > 1)
        {
            Destroy(level.LevelHolder);
            _levelEditors.Remove(level);
            _curentLevelEditor = _levelEditors[0];
            _curentLevelEditor.Enabled = true;
            _curentLevelEditor.LevelHolder.SetActive(true);
            return true;
        }
        return false;
    }

    public LevelEditorToolType GetCurentTool()
    {
        return _curentLevelEditor.CurrentTool;
    }

    public LevelEditorItem GetCurentItem()
    {
        return _curentLevelEditor.CurrentItem;
    }

    private void OnPointerStateChanged(bool arg)
    {
        _curentLevelEditor.ControllEnabled = arg;
        _levelEditorCamera.SetActiveControll(arg);
    }

    private void BuildInventoryTabs()
    {
        foreach (LevelEditorItemsDB.ItemGroup itemGroup in _levelLoader.ItemDB.ItemGroups)
        {
            Instantiate(_tabPrefab, _inventoryPanel).Build(itemGroup, this);
        }
    }

    private void CreateNewLevel()
    {
        LevelEditorGenerator level = new LevelEditorGenerator();
        _levelEditors.Add(level);
        _curentLevelEditor = level;
        _curentLevelEditor.Enabled = true;
        CreateFileTab(level);
    }

    private void OpenLevel()
    {
        string path = EditorUtility.OpenFilePanel("", "", "");
        _curentLevelEditor.Enabled = false;
        _curentLevelEditor.LevelHolder.SetActive(false);

        _levelLoader.LoadLevel(path, out GameObject levelHolder, out List<LevelEditorItem> levelItems);
        LevelEditorGenerator level = new LevelEditorGenerator(levelHolder, levelItems);
        _curentLevelEditor = level;
        _levelEditors.Add(level);
        _curentLevelEditor.Enabled = true;
        CreateFileTab(level);
    }

    private void SaveLevel()
    {
        string path = EditorUtility.SaveFilePanel("", "", $"{_curentLevelEditor.LevelHolder.name}.json", "");
        _levelLoader.SaveLevel(path, _curentLevelEditor.InstancedItems);
    }

    private void CreateFileTab(LevelEditorGenerator level)
    {
        Instantiate(_fileTabPrefab, _fileTabsHolder).Build(level, this);
    }

}
