using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class GameBoard : MonoBehaviour
{
    public GameObject blockPrefab;
    public const int GRID_HEIGHT = 8;
    public const int GRID_WIDTH = 7;
    public static Block[,] grid = new Block[GRID_WIDTH, GRID_HEIGHT];

    private ChainChecker chainChecker = new ChainChecker();
    private bool isChaining = false;

    public delegate void OnChainsFound(List<Chain> matchedChains, bool isChaining);
    public OnChainsFound onChainsFound;

    public delegate void OnPowerBlockDestroyed(Block block);
    public OnPowerBlockDestroyed onPowerBlockDestroyed;

    public delegate void OnTurnOver(List<Block> powerBlocks);
    public OnTurnOver onTurnOver;

    void Start()
    {
        AddBordersToGrid();
    }

    private void AddBordersToGrid()
    {
        List<Block> borders = new List<Block>();
        borders.Add(Instantiate(blockPrefab, new Vector2(0, 0), Quaternion.identity).GetComponent<Block>());
        borders.Add(Instantiate(blockPrefab, new Vector2(1, 0), Quaternion.identity).GetComponent<Block>());
        borders.Add(Instantiate(blockPrefab, new Vector2(0, 1), Quaternion.identity).GetComponent<Block>());
        borders.Add(Instantiate(blockPrefab, new Vector2(0, 2), Quaternion.identity).GetComponent<Block>());
        borders.Add(Instantiate(blockPrefab, new Vector2(GRID_WIDTH - 1, 0), Quaternion.identity).GetComponent<Block>());
        borders.Add(Instantiate(blockPrefab, new Vector2(GRID_WIDTH - 2, 0), Quaternion.identity).GetComponent<Block>());
        borders.Add(Instantiate(blockPrefab, new Vector2(GRID_WIDTH - 1, 1), Quaternion.identity).GetComponent<Block>());
        borders.Add(Instantiate(blockPrefab, new Vector2(GRID_WIDTH - 1, 2), Quaternion.identity).GetComponent<Block>());

        foreach (Block block in borders)
        {
            block.blockType = BlockType.Border;
            AddToGrid(block);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGridDebug();
    }

    private void UpdateGridDebug()
    {
        Text text = GameObject.Find("GridDebug").GetComponent<Text>();
        StringBuilder sb = new StringBuilder();

        for (int i = GRID_HEIGHT - 1; i >= 0; i--)
        {
            for (int j = 0; j < GRID_WIDTH; j++)
            {
                Block block = grid[j, i];

                if (block != null)
                {

                    BlockType blockType = grid[j, i].GetComponent<Block>().blockType;
                    if (blockType == BlockType.Black)
                        sb.Append("b");
                    else if (blockType == BlockType.White)
                        sb.Append("w");
                    else if (blockType == BlockType.Border)
                        sb.Append("o");
                    else if (block.isPowerBlock())
                        sb.Append("*");
                }
                else
                {
                    sb.Append("x");
                }

                if (j == GRID_WIDTH - 1)
                {
                    sb.Append("\n");
                }
                else
                {
                    sb.Append(" ");
                }

            }
        }

        text.text = sb.ToString();
    }

    public void OnBlockGroupDropped(GameObject blockGroup)
    {
        Debug.Log("OnBlockGroupDropped");
        foreach (Transform block in blockGroup.transform)
        {
            MoveBlockDown(block.GetComponent<Block>());
            AddToGrid(block.GetComponent<Block>());
        }

        CheckForMatches();
        DestroyFullColumns();
    }

    private void MoveBlockDown(Block block)
    {
        while (BlockCanMoveDown(block))
        {
            block.transform.position += new Vector3(0, -1, 0);
        }

        CheckBlockHitBottom(block);
    }

    private void CheckBlockHitBottom(Block block)
    {
        bool hitBottom = false;
        int xPos = Mathf.RoundToInt(block.transform.position.x);
        int yPos = Mathf.RoundToInt(block.transform.position.y);
        if (yPos > 0)
        {
            Block blockBelow = grid[xPos, yPos - 1];
            hitBottom = blockBelow != null && blockBelow.blockType == BlockType.Border;
        }
        else
        {
            hitBottom = true;
        }

        if (hitBottom && block.isPowerBlock())
        {
            Debug.Log("Power block hit bottom - destroy");
            DestroyBlocks(new HashSet<Block>() { block });
            onPowerBlockDestroyed(block);
        }
    }

    private bool BlockCanMoveDown(Block block)
    {
        int xPos = Mathf.RoundToInt(block.transform.position.x);
        int yPos = Mathf.RoundToInt(block.transform.position.y) - 1;

        bool isWithinGrid = xPos >= 0 && xPos <= GameBoard.GRID_WIDTH && yPos >= 0 && yPos <= GameBoard.GRID_HEIGHT;

        return isWithinGrid && grid[xPos, yPos] == null;
    }


    private void AddToGrid(Block block)
    {

        int roundedX = Mathf.RoundToInt(block.transform.position.x);
        int roundedY = Mathf.RoundToInt(block.transform.position.y);

        if (roundedX >= 0 && roundedX <= GameBoard.GRID_WIDTH && roundedY >= 0 && roundedY <= GameBoard.GRID_HEIGHT)
        {
            GameBoard.grid[roundedX, roundedY] = block;
        }
    }

    private void CheckForMatches()
    {
        List<Chain> chains = chainChecker.GetChains(grid);

        if (chains.Count == 0)
        {
            Debug.Log("No matches found - turn over");
            isChaining = false;
            onTurnOver(GetPowerBlocksInGrid());
        }
        else
        {
            onChainsFound(chains, isChaining);

            HashSet<Block> blocksToDestroy = new HashSet<Block>();

            foreach (Chain chain in chains)
            {
                blocksToDestroy.UnionWith(chain.blocks);
            }
            Debug.Log("Chains found - destroy");
            DestroyBlocks(blocksToDestroy);
        }

    }

    private List<Block> GetPowerBlocksInGrid()
    {
        List<Block> powerBlocks = new List<Block>();

        for (int x = 0; x < GRID_WIDTH; x++)
        {

            for (int y = 0; y < GRID_HEIGHT; y++)
            {
                Block block = grid[x, y];

                if (block != null && block.isPowerBlock())
                {
                    powerBlocks.Add(block);
                }
            }
        }
        return powerBlocks;
    }

    public void DestroyBlocks(HashSet<Block> blocks)
    {
        foreach (Block block in blocks)
        {
            int xPos = Mathf.RoundToInt(block.transform.position.x);
            int yPos = Mathf.RoundToInt(block.transform.position.y);

            Destroy(block.gameObject, 1.0f);
            grid[xPos, yPos] = null;
        }
        Invoke("MoveAllBlocksDown", 1.1f);
    }

    private void MoveAllBlocksDown()
    {
        Debug.Log("Moving all blocks down");
        for (int y = 1; y < GRID_HEIGHT; y++)
        {
            for (int x = 0; x < GRID_WIDTH; x++)
            {
                Block block = grid[x, y];

                if (block != null)
                {
                    int movesDown = 0;

                    for (int j = y - 1; j >= 0; j--)
                    {
                        if (grid[x, j] == null)
                        {
                            movesDown++;
                        }
                    }
                    if (movesDown > 0)
                    {
                        block.transform.position -= new Vector3(0, movesDown, 0);
                        grid[x, y - movesDown] = grid[x, y];
                        grid[x, y] = null;
                        CheckBlockHitBottom(block);
                    }

                }
            }
        }
        CheckForMatches();
    }

    private void DestroyFullColumns()
    {
        for (int x = 0; x < GRID_WIDTH; x++)
        {
            bool isFull = true;

            for (int y = 0; y < GRID_HEIGHT; y++)
            {
                if (grid[x, y] == null)
                {
                    isFull = false;
                }
            }

            if (isFull)
            {
                HashSet<Block> blocksToDestroy = new HashSet<Block>();
                for (int y = 0; y < GRID_HEIGHT; y++)
                {
                    Block block = grid[x, y];

                    if (block.blockType == BlockType.Black || block.blockType == BlockType.White)
                    {
                        blocksToDestroy.Add(block);
                    }
                }
                Debug.Log("Column full - destroy");
                DestroyBlocks(blocksToDestroy);
            }
        }
    }
}
