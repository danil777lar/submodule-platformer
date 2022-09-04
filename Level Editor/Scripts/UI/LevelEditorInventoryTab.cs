using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class LevelEditorInventoryTab : MonoBehaviour
{
    private static List<LevelEditorInventoryTab> _instances = new List<LevelEditorInventoryTab>();

    [SerializeField] private Image _image;
    [SerializeField] private Image _buttonImage;
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private Color _selectedColor;
    [Header("Prefabs")]
    [SerializeField] private RectTransform _inventoryPrefab;
    [SerializeField] private LevelEditorItemButton _itemButtonPrefab;

    private float _animDuration = 0.25f;
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


    private void Update()
    {
        Color targetColor = _mainUI.GetCurentItem() && _itemGroup.Items.Contains(_mainUI.GetCurentItem()) ? _selectedColor : Color.white;
        _buttonImage.color = Color.Lerp(_buttonImage.color, targetColor, Time.deltaTime * 2.5f);
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
            tab.CloseTab(true);
        }

        _inventoryInstance = Instantiate(_inventoryPrefab, _mainUI.Root);
        _inventoryInstance.GetComponentInChildren<TextMeshProUGUI>().text = _itemGroup.GroupName;
        _inventoryInstance.GetComponentInChildren<Button>().onClick.AddListener(() => CloseTab());
        Vector2 startPosition = new Vector2(-_inventoryInstance.sizeDelta.x / 2f, transform.parent.position.y + ((RectTransform)transform.parent).sizeDelta.y); 
        Vector2 endPosition = new Vector2(_inventoryInstance.sizeDelta.x / 2f, transform.parent.position.y + ((RectTransform)transform.parent).sizeDelta.y);
        _inventoryInstance.position = startPosition;
        _inventoryInstance.localScale = Vector3.one * 0.75f;
        _inventoryInstance.DOScale(1f, _animDuration)
            .SetTarget(_inventoryInstance)
            .SetEase(Ease.OutQuad);
        _inventoryInstance.DOMove(endPosition, _animDuration)
            .SetEase(Ease.OutBack)
            .SetTarget(_inventoryInstance);


        ScrollRect scroll = _inventoryInstance.GetComponentInChildren<ScrollRect>();
        foreach (LevelEditorItem item in _itemGroup.Items) 
        {
            Instantiate(_itemButtonPrefab, scroll.content).Build(_mainUI, item);
        }
    }


    private void CloseTab(bool hasAnotherInventory = false) 
    {
        if (!_isOpened) return;
        _isOpened = false;


        Vector2 endPosition;
        endPosition.x = _inventoryInstance.sizeDelta.x * (hasAnotherInventory ? 1.5f : -0.5f);
        endPosition.y = transform.parent.position.y + ((RectTransform)transform.parent).sizeDelta.y;
        _inventoryInstance.DOKill();
        _inventoryInstance.DOScale(0.75f, _animDuration)
            .SetEase(Ease.InQuad)
            .SetTarget(_inventoryInstance);
        _inventoryInstance.GetComponent<CanvasGroup>().DOFade(0f, _animDuration);
        _inventoryInstance.DOMove(endPosition, _animDuration)
            .SetEase(Ease.Linear)
            .OnKill(() => Destroy(_inventoryInstance.gameObject))
            .OnComplete(() => Destroy(_inventoryInstance.gameObject));
    }
}
