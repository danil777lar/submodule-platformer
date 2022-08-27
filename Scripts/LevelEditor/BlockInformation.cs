using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/BlockInformation")]
public class BlockInformation : ScriptableObject
{
    [Flags] public enum CoverSide 
    {
        None = 0,
        Top = 1, 
        Bottom = 2,
        Left = 4,
        Right = 8
    }

    [SerializeField] private string _name;
    [SerializeField] private List<Sprite> _mainSprites;
    [SerializeField] private List<Sprite> _topCoverSprites;
    [SerializeField] private List<Sprite> _bottomCoverSprites;
    [SerializeField] private List<Sprite> _leftCoverSprites;
    [SerializeField] private List<Sprite> _rightCoverSprites;


    public string Name => _name;
    public Sprite Preview => _mainSprites[0];


    public GameObject BuildSprite(CoverSide cover, Color colorMod)
    {
        SpriteRenderer mainSprite = SpawnSpriteRenderer(_mainSprites, _name, colorMod);
        int i = 0;
        foreach (int side in new int[]{ 1, 2, 4, 8 })
        {
            i++;
            if (cover.HasFlag((CoverSide)side)) 
            {
                GameObject sideSpriteInstance = SpawnSpriteRenderer(GetSideSpriteList((CoverSide)side), Enum.GetName(typeof(CoverSide), (CoverSide)side), colorMod).gameObject;
                sideSpriteInstance.transform.SetParent(mainSprite.transform);
                sideSpriteInstance.transform.localPosition = Vector3.back * i * 0.01f;
            }
        }

        return mainSprite.gameObject;
    }

    private SpriteRenderer SpawnSpriteRenderer(List<Sprite> spriteList, string name, Color colorMod) 
    {
        SpriteRenderer sprite = new GameObject(name).AddComponent<SpriteRenderer>();
        sprite.sprite = spriteList[UnityEngine.Random.Range(0, spriteList.Count)];
        sprite.color = colorMod;
        return sprite;
    }

    private List<Sprite> GetSideSpriteList(CoverSide side) 
    {
        switch (side) 
        {
            case CoverSide.Top: return _topCoverSprites;
            case CoverSide.Bottom: return _bottomCoverSprites;
            case CoverSide.Left: return _leftCoverSprites;
            case CoverSide.Right: return _rightCoverSprites;
            default: return null;
        }
    }
}
