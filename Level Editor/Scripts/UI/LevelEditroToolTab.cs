using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LevelEditroToolTab : MonoBehaviour
{
    [SerializeField] private LevelEditorGenerator.LevelEditorToolType _toolType;
    [SerializeField] private Image _image;
    private LevelEditorUI _mainUI;


    private void Start()
    {
        _mainUI = GetComponentInParent<LevelEditorUI>();
        GetComponent<Button>().onClick.AddListener(OnButtonClicked);
    }

    private void Update()
    {
        _image.color = _mainUI.GetCurentTool() == _toolType ? Color.gray : Color.white;
    }

    private void OnButtonClicked() 
    {
        _mainUI.SetCurentTool(_toolType);
    }
}
