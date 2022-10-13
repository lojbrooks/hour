using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Chain
{
    public Chain(Block[] blocks, BlockType chainColor)
    {
        this.blocks = blocks;
        this.chainColor = chainColor;
    }

    public Block[] blocks { get; }
    public BlockType chainColor { get; }
}
