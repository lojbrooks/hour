using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class BlockMovement : MonoBehaviour
{
    bool overGridSlot;
    bool canMove;
    bool dragging;
    public bool droppedBlock;
    
   
    public GameObject landedBlock;
    Collider2D collider;
    private Vector3 resetPosition;

    void Start()
    {
        collider = GetComponent <Collider2D>();
        canMove = false;
        dragging = false;
        resetPosition = this.transform.localPosition;
        overGridSlot = false;
        droppedBlock = false;
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
             

        if (Input.GetMouseButtonDown(0))
        {
            
            if (collider == Physics2D.OverlapPoint(mousePos))
            {
                canMove = true;
            }
            else
            {
                canMove = false;
            }
            if (canMove)
            {
                dragging = true;
                droppedBlock = false;
            }
            
            
            

        }
        if (dragging)
        {   
            this.transform.position = mousePos;
            if (mousePos.x > 1 && mousePos.x < 2)
            {
                this.transform.position = new Vector3(1, 7, 0);
                overGridSlot = true;
            }
            else if (mousePos.x > 2 && mousePos.x < 3)
            {
                this.transform.position = new Vector3(2, 7, 0);
                overGridSlot = true;
            }
            else if (mousePos.x > 3 && mousePos.x < 4)
            {
                this.transform.position = new Vector3(3, 7, 0);
                overGridSlot = true;
            }
            else if (mousePos.x > 4 && mousePos.x < 5)
            {
                this.transform.position = new Vector3(4, 7, 0);
                overGridSlot = true;
            }
            else if (mousePos.x > 5 && mousePos.x < 6)
            {
                this.transform.position = new Vector3(5, 7, 0);
                overGridSlot = true;
            }
            else
                overGridSlot = false;

            
        }
        if (Input.GetMouseButtonUp(0))
        {
            canMove = false;
            dragging = false;
            if (!overGridSlot)
            {
                this.transform.position = new Vector3(resetPosition.x, resetPosition.y, resetPosition.z);
            }
            else
            {

                transform.DetachChildren();
                this.transform.position = new Vector3(resetPosition.x, resetPosition.y, resetPosition.z);
                FindObjectOfType<SpawnBlock>().SpawnBlocksAtLeftPosition();
                
                
               
                overGridSlot = false;
                droppedBlock = true;
                
                
            }
            


        }  
    }

    
}

