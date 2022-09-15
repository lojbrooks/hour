using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainChecker
{
    public List<Chain> GetChains(GameObject[,] grid)
    {
        List<Chain> chains = new List<Chain>();
        int startPos = 0;
        int chainCount = 0;
        string chainColor = "";
        int gridWidth = grid.GetLength(0);
        int gridHeight = grid.GetLength(1);

        Debug.Log("Checking chains " + gridWidth + " " + gridHeight);

        // check horizontal chains
        for (int y = 0; y < gridHeight; y++)
        {

            for (int x = 0; x < gridWidth; x++)
            {
                GameObject block = grid[x, y];

                if (IsChainableBlock(block))
                {
                    if(chainCount == 0)
                    {
                        startPos = x;
                        chainCount = 1;
                        chainColor = block.tag;
                        Debug.Log("New " + chainColor + " chain");
                    }
                    else if(block.tag == chainColor)
                    {
                        chainCount++;
                    }
                }

                bool endOfChain = x == gridWidth - 1 || !IsChainableBlock(grid[x + 1, y]) || grid[x + 1, y].tag != chainColor;

                if (endOfChain)
                {
                    if (chainCount >= 3)
                    {
                        GameObject[] blocks = new GameObject[chainCount];

                        for (int j = 0; j < chainCount; j++)
                        {
                            blocks[j] = grid[startPos + j, y];
                        }

                        Debug.Log("Chain found: " + blocks.Length);

                        chains.Add(new Chain(blocks, chainColor));
                        chainCount = 0;
                        chainColor = "";
                    }
                    else
                    {
                        if(chainCount > 0)
                        {
                            Debug.Log("Chain too small: " + chainCount);
                           
                        }
                        
                        chainCount = 0;
                        chainColor = "";
                    }
                }
            }
        }

        Debug.Log("Returning " + chains.Count + " chains");
        return chains;
    }

    private bool IsChainableBlock(GameObject block)
    {
        return block != null && (block.tag == "Black" || block.tag == "White");
    }
}


