using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BlockType { white, black, border };

public class Block : MonoBehaviour
{
    
    public static int gridHeight = 8;
    public static int gridWidth = 7;
    public static Transform[,] grid = new Transform[gridWidth, gridHeight];
    

    public BlockType blockType = BlockType.border;

    void Start()
    {
        AddToGrid();
        
    }

    void Update()
    {
        
        if (transform.position.y == 7)
        {
            if (FindObjectOfType<BlockMovement>().droppedBlock == true)
            {
                MoveBlocksDown();
                  
                  
            }
        }
        
        
        
     
    }
    void MoveBlocksDown()
    {
        bool moveDown = true;

        while (moveDown == true)
        {
            transform.position += new Vector3(0, -1, 0);
            if (!ValidMove())
            {
                transform.position += new Vector3(0, 1, 0);
                moveDown = false;
            }
        }
        
        AddToGrid();
        
       
    }

    

    bool ValidMove()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            if (roundedY < 0)

            {
                return false;
            }

            if (grid[roundedX, roundedY] != null)
                return false;
        }
        return true;
    }

    void AddToGrid()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            if (roundedX >= 0 && roundedX <= gridWidth && roundedY >= 0 && roundedY <= gridHeight)
            {
                grid[roundedX, roundedY] = children;
            }
            
           
        }


    }

    

    
  
}

