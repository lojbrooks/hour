using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockType { Empty, White, Black, Red, Orange, Blue, Green, Indigo, Violet, Border };

public class Block : MonoBehaviour
{   

    public BlockType blockType;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        switch(blockType)
        {
            case BlockType.White:
                spriteRenderer.color = Color.white;
                break;
            case BlockType.Black:
                spriteRenderer.color = Color.black;
                break;
            case BlockType.Red:
                spriteRenderer.color = Color.red;
                break;
            case BlockType.Border:
                spriteRenderer.color = new Color(0.46f, 0.24f, 0.14f, 1f);
                break;
        }
    }

    public bool isPowerBlock()
    {
        return blockType != BlockType.White
            && blockType != BlockType.Black
            && blockType != BlockType.Border
            && blockType != BlockType.Empty;
    }

}

