using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelEditorUI : MonoBehaviour
{
    [SerializeField] private Button _itemButtonPrefab;
    [Header("Blocks")]
    [SerializeField] private ScrollRect _blockScroll;
    [SerializeField] private List<BlockInformation> _blockList;

    public bool IsClosed => !transform.GetChild(0).gameObject.activeSelf;
    public Action OnButtonSavePressed;
    public Action<BlockInformation> OnBlockChoosed;


    private void Start()
    {
        foreach (BlockInformation blockInfo in _blockList) 
        {
            Button itemButtonInstance = Instantiate(_itemButtonPrefab, _blockScroll.content);
            itemButtonInstance.GetComponentsInChildren<Image>()[1].sprite = blockInfo.Preview;
            itemButtonInstance.GetComponentInChildren<TextMeshProUGUI>().text = blockInfo.Name;
            itemButtonInstance.onClick.AddListener(() => OnBlockChoosed?.Invoke(blockInfo));
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) 
        {
            transform.GetChild(0).gameObject.SetActive(!transform.GetChild(0).gameObject.activeSelf);
        }
    }
}
