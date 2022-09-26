using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class GameBoard : MonoBehaviour
{

    public const int GRID_HEIGHT = 8;
    public const int GRID_WIDTH = 7;
    public static GameObject[,] grid = new GameObject[GRID_WIDTH, GRID_HEIGHT];

    private ChainChecker chainChecker = new ChainChecker();

    public delegate void OnTurnOver(List<Chain> matchedChains);
    public OnTurnOver onTurnOver;
   
    void Start()
    {
        AddBordersToGrid();
    }

    private void AddBordersToGrid()
    {
        GameObject[] borders = GameObject.FindGameObjectsWithTag("Border");

        foreach (GameObject border in borders)
        {
            AddToGrid(border);
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
                GameObject block = grid[j, i];
                if (block != null)
                {
                    string tag = block.tag;

                    if (tag == "Border")
                        sb.Append("o");
                    else if (tag == "Black")
                        sb.Append("b");
                    else if (tag == "White")
                        sb.Append("w");
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
        foreach (Transform block in blockGroup.transform)
        {
            MoveBlockDown(block.gameObject);
            AddToGrid(block.gameObject);
        }

        CheckForMatches();
        DestroyFullColumns();
    }

    private void MoveBlockDown(GameObject block)
    {
        while (BlockCanMoveDown(block))
        {
            block.transform.position += new Vector3(0, -1, 0);
        }
    }

    private bool BlockCanMoveDown(GameObject block)
    {
        int xPos = Mathf.RoundToInt(block.transform.position.x);
        int yPos = Mathf.RoundToInt(block.transform.position.y) - 1;

        bool isWithinGrid = xPos >= 0 && xPos <= GameBoard.GRID_WIDTH && yPos >= 0 && yPos <= GameBoard.GRID_HEIGHT;

        return isWithinGrid && grid[xPos, yPos] == null;
    }


    private void AddToGrid(GameObject block)
    {

        int roundedX = Mathf.RoundToInt(block.transform.position.x);
        int roundedY = Mathf.RoundToInt(block.transform.position.y);

        if (roundedX >= 0 && roundedX <= GameBoard.GRID_WIDTH && roundedY >= 0 && roundedY <= GameBoard.GRID_HEIGHT)
        {
            Debug.Log("Adding " + block.tag + " to grid: x = " + roundedX + " y = " + roundedY);
            GameBoard.grid[roundedX, roundedY] = block;
        }
    }

    private void CheckForMatches()
    {
        List<Chain> chains = chainChecker.GetChains(grid);
        onTurnOver(chains);
    }

    public void DestroyBlocks(HashSet<GameObject> blocks)
    {
        foreach(GameObject block in blocks)
        {
            int xPos = Mathf.RoundToInt(block.transform.position.x);
            int yPos = Mathf.RoundToInt(block.transform.position.y);

            Destroy(block, 1.0f);
            grid[xPos, yPos] = null;
        }
        Invoke("MoveAllBlocksDown", 1.1f);
    }

    private void MoveAllBlocksDown()
    {
        for (int y = 1; y < GRID_HEIGHT; y++)
        {
            for (int x = 0; x < GRID_WIDTH; x++)
            {
                GameObject block = grid[x, y];

                if (block != null)
                {
                    int movesDown = 0;

                    for(int j = y-1; j >= 0; j--)
                    {
                        if(grid[x,j] == null)
                        {
                            movesDown++;
                        }
                    }
                    if(movesDown > 0)
                    {
                        block.transform.position -= new Vector3(0, movesDown, 0);
                        grid[x, y - movesDown] = grid[x, y];
                        grid[x, y] = null;
                    }
                    
                }
            }
        }
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
                for (int y = 0; y < GRID_HEIGHT; y++)
                {
                    GameObject block = grid[x, y];

                    if (block.tag != "Border")
                    {
                        Destroy(block);
                        grid[x, y] = null;
                    }
                }
            }
        }
    }
}
