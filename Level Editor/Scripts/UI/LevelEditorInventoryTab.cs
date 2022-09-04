using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelEditorInventoryTab : MonoBehaviour
{
    private static List<LevelEditorInventoryTab> _instances = new List<LevelEditorInventoryTab>();

    [SerializeField] private Image _image;
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _title;
    [Header("Prefabs")]
    [SerializeField] private RectTransform _inventoryPrefab;
    [SerializeField] private Button _itemButtonPrefab;

    private bool _isOpened;
    private RectTransform _inventoryInstance;
    private LevelEditorUI _mainUI;
    private LevelEditorItemsDB.ItemGroup _itemGroup;


    public LevelEditorInventoryTab Build(LevelEditorItemsDB.ItemGroup itemGroup, LevelEditorUI mainUI) 
    {
        _itemGroup = itemGroup;
        _mainUI = mainUI;
        _image.sprite = itemGroup.GroupIcon;
        _title.text = itemGroup.GroupName;

        _button.onClick.AddListener(OnButtonClicked);
        _instances.Add(this);

        return this;
    }


    private void OnButtonClicked() 
    {
        if (_isOpened) CloseTab();
        else OpenTab();
    }

    private void OpenTab()
    {
        if (_isOpened) return;
        _isOpened = true;

        foreach (LevelEditorInventoryTab tab in _instances.FindAll((t) => t != this)) 
        {
            tab.CloseTab();
        }

        _inventoryInstance = Instantiate(_inventoryPrefab, _mainUI.Root);
        Vector2 position;
        position.x = Mathf.Clamp(transform.position.x, _inventoryInstance.sizeDelta.x / 2f, Screen.width - (_inventoryInstance.sizeDelta.x / 2f));
        position.y = transform.parent.position.y + ((RectTransform)transform.parent).sizeDelta.y;
        _inventoryInstance.position = position;
        _inventoryInstance.GetComponentInChildren<TextMeshProUGUI>().text = _itemGroup.GroupName;
        _inventoryInstance.GetComponentInChildren<Button>().onClick.AddListener(CloseTab);

        ScrollRect scroll = _inventoryInstance.GetComponentInChildren<ScrollRect>();
        foreach (LevelEditorItem item in _itemGroup.Items) 
        {
            Button itemButtonInstance = Instantiate(_itemButtonPrefab, scroll.content);
            itemButtonInstance.onClick.AddListener(() => _mainUI.SetCurentItem(item));
            itemButtonInstance.GetComponentInChildren<TextMeshProUGUI>(true).text = item.gameObject.name;
            List<Image> images = new List<Image>(itemButtonInstance.GetComponentsInChildren<Image>());
            images.Find((image) => image.gameObject.name == "Icon").sprite = item.GetPreview();
        }
    }


    private void CloseTab() 
    {
        if (!_isOpened) return;
        _isOpened = false;
        Destroy(_inventoryInstance.gameObject);
    }
}
