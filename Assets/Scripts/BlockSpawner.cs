using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner
{

    public GameObject blockPrefab;

    public BlockType GetBlockTypeForLevel(int level)
    {
        BlockType result = BlockType.Empty;

        switch(level)
        {
            case 1:
                result = GetLevel1BlockType();
                break;
            case 2:
                result = GetLevel2BlockType();
                break;
            default:
                result = GetLevel2BlockType();
                break;
        }

        return result;
    }

    private BlockType GetLevel1BlockType()
    {
        int randomPercent = Random.Range(1, 101);
        BlockType blockType;
        if (randomPercent < 50)
        {
            blockType = BlockType.White;
        }
        else
        {
            blockType = BlockType.Black;
        }

        return blockType;
    }

    private BlockType GetLevel2BlockType()
    {
        int randomPercent = Random.Range(1, 101);
        BlockType blockType;
        if (randomPercent < 48)
        {
            blockType = BlockType.White;
        }
        else if (randomPercent < 96)
        {
            blockType = BlockType.Black;
        } else
        {
            blockType = BlockType.Red;
        }

        return blockType;
    }
}
