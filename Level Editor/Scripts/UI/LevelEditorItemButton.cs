using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class LevelEditorItemButton : MonoBehaviour
{
    static private Action<LevelEditorItemButton> SomeButtonClicked;

    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private Image _icon;
    [SerializeField] private RectTransform _iconHolder;
    [Header("Animation")]
    [SerializeField] private float _inDuration;
    [SerializeField] private float _outDuration;
    [SerializeField] private Ease _inEase;
    [SerializeField] private Ease _outEase;

    private float _iconHolderDefaultSize;
    private LevelEditorItem _item;
    private LevelEditorUI _mainUI;


    public void Build(LevelEditorUI mainUI, LevelEditorItem item) 
    {
        _mainUI = mainUI;
        _item = item;
        _button.onClick.AddListener(OnButtonCLicked);
        _title.GetComponentInChildren<TextMeshProUGUI>(true).text = item.gameObject.name;
        _icon.sprite = item.GetPreview();
        _iconHolderDefaultSize = _iconHolder.sizeDelta.x;

        if (_mainUI.GetCurentItem() != null && _mainUI.GetCurentItem() == _item) 
        {
            SetSelectedView();
        }

        SomeButtonClicked += OnSomeButtonCLicked;
    }

    private void OnButtonCLicked() 
    {
        _mainUI.SetCurentItem(_item);
        SetSelectedView();
        SomeButtonClicked?.Invoke(this);
    }

    private void OnSomeButtonCLicked(LevelEditorItemButton clickedButton) 
    {
        if (clickedButton != this) 
        {
            SetDeselectedView();
        }
    }

    private void SetSelectedView() 
    {
        this.DOKill();
        _iconHolder.DOSizeDelta(Vector2.one * 0f, _inDuration)
            .SetEase(_inEase)
            .SetTarget(this);
    }

    private void SetDeselectedView() 
    {
        this.DOKill();
        _iconHolder.DOSizeDelta(Vector2.one * _iconHolderDefaultSize, _outDuration)
            .SetEase(_outEase)
            .SetTarget(this);
    }
}
