using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBlock : MonoBehaviour
{

    public GameObject[] blocks;
    public Transform parent;
    public Transform parentB;

    void Start()
    {
        SpawnBlocksAtLeftPosition();
        SpawnBlocksAtRightPosition();

    }

    public void SpawnBlocksAtLeftPosition()
    {
        Vector3[] spawnPositions = new[] { new Vector3(-5, 0, 0), new Vector3(-4, 0, 0), new Vector3(-3, 0, 0) };

        for (int i = 0; i < 3; i++)
        {
            GameObject newObjectLeft = Instantiate(blocks[Random.Range(0, 2)], spawnPositions[i], Quaternion.identity);
            newObjectLeft.transform.SetParent(parent);
            
        }
    }

    public void SpawnBlocksAtRightPosition()
    {
        Vector3[] spawnPositions = new[] { new Vector3(9, 0, 0), new Vector3(10, 0, 0), new Vector3(11, 0, 0) };

        for (int i = 0; i < 3; i++)
        {
            GameObject newObjectRight = Instantiate(blocks[Random.Range(0, 2)], spawnPositions[i], Quaternion.identity);
            newObjectRight.transform.SetParent(parentB);
        }
    }
   
 }
