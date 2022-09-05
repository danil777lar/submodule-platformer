using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

[RequireComponent(typeof(Button))]
public class LevelEditorFileTab : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelTitle;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Image _background;
    [Header("Colors")]
    [SerializeField] private Color _selectedColor;
    [SerializeField] private Color _notSelectedColor;

    private LevelEditorGenerator _level;
    private LevelEditorUI _mainUI;



    private void Update()
    {
        Color targetColor = _level == _mainUI.CurentLevel ? _selectedColor : _notSelectedColor;
        _background.color = Color.Lerp(_background.color, targetColor, Time.deltaTime * 5f);
        _closeButton.image.color = Color.Lerp(_closeButton.image.color, targetColor, Time.deltaTime * 5f);
    }


    public void Build(LevelEditorGenerator level, LevelEditorUI mainUI)
    {
        _level = level;
        _mainUI = mainUI;

        _levelTitle.text = level.LevelHolder.name;
        _closeButton.onClick.AddListener(OnCloseButtonClicked);
        GetComponent<Button>().onClick.AddListener(OnButtonCLicked);
    }


    private void OnCloseButtonClicked() 
    {
        if (_mainUI.CloseLevel(_level)) 
        {
            this.DOKill();
            Vector2 targetSize = ((RectTransform)transform).sizeDelta;
            targetSize.x = 25f;
            ((RectTransform)transform).DOSizeDelta(targetSize, 0.5f)
                .SetEase(Ease.InBack)
                .OnComplete(() => 
                {
                    ((RectTransform)_background.transform).DOAnchorMax(new Vector2(1f, 0f), 0.1f)
                        .OnComplete(() => 
                        {
                            
                        });
                    ((RectTransform)transform).DOSizeDelta(Vector2.zero, 0.1f)
                                .OnComplete(() => Destroy(gameObject));
                });
        }
    }

    private void OnButtonCLicked() 
    {
        _mainUI.ChooseLevel(_level);

        this.DOKill();
        Vector2 defaultSize = ((RectTransform)transform).sizeDelta;
        Vector2 targetSize = ((RectTransform)transform).sizeDelta;
        targetSize.x *= 0.75f;
        ((RectTransform)transform).DOSizeDelta(targetSize, 0.1f)
            .SetTarget(this)
            .OnComplete(() => 
            {
                ((RectTransform)transform).DOSizeDelta(defaultSize, 0.25f)
                    .SetEase(Ease.OutBack)
                    .SetTarget(this);
            });
    }
}
