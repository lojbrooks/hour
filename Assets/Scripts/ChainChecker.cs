using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainChecker
{
    public List<Chain> GetChains(Block[,] grid)
    {
        List<Chain> chains = new List<Chain>();
        
        chains.AddRange(GetHorizontalChains(grid));
        chains.AddRange(GetVerticalChains(grid));

        return chains;
    }

    private List<Chain> GetHorizontalChains(Block[,] grid)
    {
        List<Chain> chains = new List<Chain>();
        int startPos = 0;
        int chainCount = 0;
        BlockType chainColor = BlockType.Empty;
        int gridWidth = grid.GetLength(0);
        int gridHeight = grid.GetLength(1);

        for (int y = 0; y < gridHeight; y++)
        {

            for (int x = 0; x < gridWidth; x++)
            {
                Block block = grid[x, y];

                if (IsChainableBlock(block))
                {
                    if (chainCount == 0)
                    {
                        startPos = x;
                        chainCount = 1;
                        chainColor = block.blockType;
                    }
                    else if (block.blockType == chainColor)
                    {
                        chainCount++;
                    }
                }

                bool endOfChain = x == gridWidth - 1 || !IsChainableBlock(grid[x + 1, y]) || grid[x + 1, y].blockType != chainColor;

                if (endOfChain)
                {
                    if (chainCount >= 3)
                    {
                        Block[] blocks = new Block[chainCount];

                        for (int j = 0; j < chainCount; j++)
                        {
                            blocks[j] = grid[startPos + j, y];
                        }

                        chains.Add(new Chain(blocks, chainColor));
                        chainCount = 0;
                        chainColor = BlockType.Empty;
                    }
                    else
                    {
                        chainCount = 0;
                        chainColor = BlockType.Empty;
                    }
                }
            }
        }

        return chains;

    }

    private List<Chain> GetVerticalChains(Block[,] grid)
    {
        List<Chain> chains = new List<Chain>();
        int startPos = 0;
        int chainCount = 0;
        BlockType chainColor = BlockType.Empty;
        int gridWidth = grid.GetLength(0);
        int gridHeight = grid.GetLength(1);

        for (int x = 0; x < gridWidth; x++)
        {

            for (int y = 0; y < gridHeight; y++)
            {
                Block block = grid[x, y];

                if (IsChainableBlock(block))
                {
                    if (chainCount == 0)
                    {
                        startPos = y;
                        chainCount = 1;
                        chainColor = block.blockType;
                    }
                    else if (block.blockType == chainColor)
                    {
                        chainCount++;
                    }
                }

                bool endOfChain = y == gridHeight - 1 || !IsChainableBlock(grid[x, y + 1]) || grid[x, y + 1].blockType != chainColor;

                if (endOfChain)
                {
                    if (chainCount >= 3)
                    {
                        Block[] blocks = new Block[chainCount];

                        for (int j = 0; j < chainCount; j++)
                        {
                            blocks[j] = grid[x, startPos + j];
                        }

                        chains.Add(new Chain(blocks, chainColor));
                        chainCount = 0;
                        chainColor = BlockType.Empty;
                    }
                    else
                    {
                        chainCount = 0;
                        chainColor = BlockType.Empty;
                    }
                }
            }
        }

        return chains;

    }

    private bool IsChainableBlock(Block block)
    {
        return block != null && (block.blockType == BlockType.Black || block.blockType == BlockType.White);
    }
}


