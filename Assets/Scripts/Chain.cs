using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Chain
{
    public Chain(GameObject[] blocks, string chainColor)
    {
        this.blocks = blocks;
        this.chainColor = chainColor;
    }

    public GameObject[] blocks { get; }
    public string chainColor { get; }
}
