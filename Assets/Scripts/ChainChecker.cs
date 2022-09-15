using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainChecker
{
    public List<Chain> GetChains(GameObject[,] grid)
    {
        List<Chain> chains = new List<Chain>();
        
        chains.AddRange(GetHorizontalChains(grid));
        chains.AddRange(GetVerticalChains(grid));

        return chains;
    }

    private List<Chain> GetHorizontalChains(GameObject[,] grid)
    {
        List<Chain> chains = new List<Chain>();
        int startPos = 0;
        int chainCount = 0;
        string chainColor = "";
        int gridWidth = grid.GetLength(0);
        int gridHeight = grid.GetLength(1);

        for (int y = 0; y < gridHeight; y++)
        {

            for (int x = 0; x < gridWidth; x++)
            {
                GameObject block = grid[x, y];

                if (IsChainableBlock(block))
                {
                    if (chainCount == 0)
                    {
                        startPos = x;
                        chainCount = 1;
                        chainColor = block.tag;
                    }
                    else if (block.tag == chainColor)
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

                        chains.Add(new Chain(blocks, chainColor));
                        chainCount = 0;
                        chainColor = "";
                    }
                    else
                    {
                        chainCount = 0;
                        chainColor = "";
                    }
                }
            }
        }

        return chains;

    }

    private List<Chain> GetVerticalChains(GameObject[,] grid)
    {
        List<Chain> chains = new List<Chain>();
        int startPos = 0;
        int chainCount = 0;
        string chainColor = "";
        int gridWidth = grid.GetLength(0);
        int gridHeight = grid.GetLength(1);

        for (int x = 0; x < gridWidth; x++)
        {

            for (int y = 0; y < gridHeight; y++)
            {
                GameObject block = grid[x, y];

                if (IsChainableBlock(block))
                {
                    if (chainCount == 0)
                    {
                        startPos = y;
                        chainCount = 1;
                        chainColor = block.tag;
                    }
                    else if (block.tag == chainColor)
                    {
                        chainCount++;
                    }
                }

                bool endOfChain = y == gridHeight - 1 || !IsChainableBlock(grid[x, y + 1]) || grid[x, y + 1].tag != chainColor;

                if (endOfChain)
                {
                    if (chainCount >= 3)
                    {
                        GameObject[] blocks = new GameObject[chainCount];

                        for (int j = 0; j < chainCount; j++)
                        {
                            blocks[j] = grid[x, startPos + j];
                        }

                        chains.Add(new Chain(blocks, chainColor));
                        chainCount = 0;
                        chainColor = "";
                    }
                    else
                    {
                        chainCount = 0;
                        chainColor = "";
                    }
                }
            }
        }

        return chains;

    }

    private bool IsChainableBlock(GameObject block)
    {
        return block != null && (block.tag == "Black" || block.tag == "White");
    }
}


