using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class LevelEditroToolTab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private bool _enableThicknessChoose;
    [SerializeField] private LevelEditorToolType _toolType;
    [Header("Links")]
    [SerializeField] private Button _button;
    [SerializeField] private Image _image;
    [SerializeField] private RectTransform _thicknesRoot;
    [SerializeField] private TextMeshProUGUI _thicknesText;
    [SerializeField] private Slider _thicknesSlider;
    [Header("Visual")]
    [SerializeField] private Color _selectedColor;

    private bool _pointerHover;
    private int _thicknes = 1;
    private LevelEditorUI _mainUI;


    private void Start()
    {
        _mainUI = GetComponentInParent<LevelEditorUI>();
        _button.onClick.AddListener(OnButtonClicked);
        _mainUI.CurentItemChanged += OnCurentItemChanged;
        OnCurentItemChanged();
    }

    private void Update()
    {
        Color targetColor = _mainUI.GetCurentTool() == _toolType ? _selectedColor : Color.white;
        _image.color = Color.Lerp(_image.color, targetColor, Time.deltaTime * 5f);
        UpdateThicknesUI();
    }

    private void OnButtonClicked() 
    {
        _mainUI.SetCurentTool(_toolType);
        _mainUI.SetThicknes(_thicknes);
    }

    private void UpdateThicknesUI()
    {
        if (_mainUI.GetCurentTool() == _toolType && _pointerHover && _enableThicknessChoose)
        {
            _thicknesRoot.gameObject.SetActive(true);

            _thicknes += (int)Input.mouseScrollDelta.y;
            _thicknes = Mathf.Clamp(_thicknes, 1, 10);
            _mainUI.SetThicknes(_thicknes);

            _thicknesSlider.value = _thicknes;
            _thicknesText.text = _thicknes.ToString();
        }
        else 
        {
            _thicknesRoot.gameObject.SetActive(false);
        }
    }

    private void OnCurentItemChanged() 
    {
        gameObject.SetActive(_mainUI.GetCurentItem() && _mainUI.GetCurentItem().AvailableTools.HasFlag((ItemAvailableTools)(int)_toolType));
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        _pointerHover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _pointerHover = false;
    }
}