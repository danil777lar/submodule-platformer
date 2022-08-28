using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoverSide = BlockInformation.CoverSide;

public class LevelEditorGenerator : MonoBehaviour
{
    [SerializeField] private LevelEditorUI _ui;
    [SerializeField] private GameObject _levelRoot;
    [SerializeField] private Transform _frontLayer;
    [SerializeField] private Transform _backLayer;
    [SerializeField] private Transform _propLayer;

    private Camera _camera;
    private BlockInformation _currentBlock;
    private SpriteRenderer _blockPreview;

    private Dictionary<Vector2, GameObject> _frontLayerBlocks = new Dictionary<Vector2, GameObject>();
    private Dictionary<Vector2, GameObject> _backLayerBlocks = new Dictionary<Vector2, GameObject>();



    private void Start()
    {
        _camera = Camera.main;
        _ui.OnBlockChoosed += (block) => _currentBlock = block;
        _ui.OnButtonSavePressed += SaveLevel;
        _blockPreview = new GameObject("Block Preview").AddComponent<SpriteRenderer>();
        _blockPreview.color = new Color(1f, 1f, 1f, 0.5f);
    }

    private void Update()
    {
        _blockPreview.gameObject.SetActive(_currentBlock && _ui.IsClosed);
        if (_currentBlock && _ui.IsClosed) 
        {
            _blockPreview.sprite = _currentBlock.Preview;

            Vector2 curentPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            curentPosition = new Vector2(Mathf.FloorToInt(curentPosition.x), Mathf.FloorToInt(curentPosition.y));
            _blockPreview.transform.position = curentPosition;

            if (Input.GetMouseButton(0))
            {
                TrySpawnBlock(_frontLayerBlocks, _frontLayer, curentPosition, Color.white, false);
            }
            if (Input.GetMouseButton(1) && !_backLayerBlocks.ContainsKey(curentPosition))
            {
                TrySpawnBlock(_backLayerBlocks, _backLayer, curentPosition, new Color(0.5f, 0.5f, 0.5f, 1f), false);
            }
        }
    }


    private void TrySpawnBlock(Dictionary<Vector2, GameObject> blocks, Transform parent, Vector2 curentPosition, Color color, bool rewriteBlock, bool rebuildNeighbours = true) 
    {
        if (blocks.ContainsKey(curentPosition))
        {
            if (rewriteBlock)
            {
                Destroy(blocks[curentPosition]);
                blocks.Remove(curentPosition);
            }
            else 
            {
                return;
            }
        }

        CoverSide sides = ComputeCoverSides(blocks, curentPosition);
        GameObject blockInstance = _currentBlock.BuildSprite(sides, color);
        blockInstance.transform.SetParent(parent);
        blockInstance.transform.position = curentPosition;
        blockInstance.transform.localPosition = new Vector3(blockInstance.transform.localPosition.x, blockInstance.transform.localPosition.y, 0f);
        if (parent == _frontLayer)
        {
            BoxCollider2D collider = blockInstance.AddComponent<BoxCollider2D>();
            collider.size = Vector2.one;
        }
        blocks.Add(curentPosition, blockInstance);

        if (rebuildNeighbours) 
        {
            if (blocks.ContainsKey(curentPosition + Vector2.up)) TrySpawnBlock(blocks, parent, curentPosition + Vector2.up, color, true, false);
            if (blocks.ContainsKey(curentPosition + Vector2.down)) TrySpawnBlock(blocks, parent, curentPosition + Vector2.down, color, true, false);
            if (blocks.ContainsKey(curentPosition + Vector2.left)) TrySpawnBlock(blocks, parent, curentPosition + Vector2.left, color, true, false);
            if (blocks.ContainsKey(curentPosition + Vector2.right)) TrySpawnBlock(blocks, parent, curentPosition + Vector2.right, color, true, false);
        }
    }

    private CoverSide ComputeCoverSides(Dictionary<Vector2, GameObject> blocks, Vector2 position) 
    {
        CoverSide sides = CoverSide.None;
        if (!blocks.ContainsKey(position + Vector2.up)) sides |= CoverSide.Top;
        if (!blocks.ContainsKey(position + Vector2.down)) sides |= CoverSide.Bottom;
        if (!blocks.ContainsKey(position + Vector2.left)) sides |= CoverSide.Left;
        if (!blocks.ContainsKey(position + Vector2.right)) sides |= CoverSide.Right;
        return sides;
    }

    private void SaveLevel() 
    {
        
    }
}
