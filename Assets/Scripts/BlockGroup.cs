using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class BlockGroup : MonoBehaviour
{
    
    public GameObject[] blocks;

    private Collider2D blockCollider;
    private Vector3 startPosition;
    private GameBoard gameBoard;
    private bool dragging;

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
        Debug.Log("IsOverGridSlot x = " + transform.position.x + " y = " + transform.position.y);
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
            GameObject newBlock = Instantiate(blocks[Random.Range(0, blocks.Length)], spawnPos, Quaternion.identity);
            newBlock.transform.SetParent(transform);

        }
    }

    
}

