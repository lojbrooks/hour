using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class BlockGroup : MonoBehaviour
{
    public GameObject block;
    private Collider2D blockCollider;
    private Vector3 startPosition;
    private GameBoard gameBoard;
    private bool dragging;
    private BlockSpawner blockSpawner = new BlockSpawner();

    void Awake()
    {
        gameBoard = FindObjectOfType<GameBoard>();
        blockCollider = GetComponent<Collider2D>();
        dragging = false;
        startPosition = this.transform.localPosition;
    }

    private void Start()
    {
        SpawnBlocks();
    }

    void Update()
    {
        
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
       

        if (Input.GetMouseButtonDown(0))
        {
            if (blockCollider == Physics2D.OverlapPoint(mousePos))
            {
                dragging = true;
            }
            else
            {
                dragging = false;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            dragging = false;
            if (!IsOverGridSlot())
            {
                this.transform.position = startPosition;
            }
            else
            {
                gameBoard.OnBlockGroupDropped(gameObject);
                transform.DetachChildren();
                this.transform.position = startPosition;
                SpawnBlocks();
            }
            
        }
        if (dragging)
        {
            this.transform.position = mousePos;
            if (mousePos.x > 1 && mousePos.x < 6)
            {
                int xPos = (int)Mathf.Floor(mousePos.x);
                transform.position = new Vector3(xPos, 7, 0);
            }
        }
    }

    private bool IsOverGridSlot()
    {
        return transform.position.y == GameBoard.GRID_HEIGHT - 1 &&
            transform.position.x >= 1 && transform.position.x < GameBoard.GRID_WIDTH - 1;
    }

    private void SpawnBlocks()
    {
        Vector3[] spawnPositions = new[] {
            new Vector3(startPosition.x - 1, startPosition.y, 0),
            new Vector3(startPosition.x, startPosition.y, 0),
            new Vector3(startPosition.x + 1, startPosition.y, 0)
        };

        foreach (Vector3 spawnPos in spawnPositions)
        {
            GameObject newBlock = Instantiate(block, spawnPos, Quaternion.identity);
            newBlock.GetComponent<Block>().blockType = blockSpawner.GetBlockTypeForLevel(GameManager.level);
            newBlock.transform.SetParent(transform);

        }
    }

    
}

