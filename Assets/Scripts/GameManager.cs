using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private const float COMBO_INCREMENT = 0.05f;
    private float comboMultiplier = 1f;
    private String nextCombo = "White";

    private GameBoard gameBoard;

    private void Awake()
    {
        gameBoard = FindObjectOfType<GameBoard>();
        gameBoard.onTurnOver += OnTurnOver;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetNextComboColor("White");
    }

    // Update is called once per frame
    void Update()
    {
        Text comboText = GameObject.Find("ComboText").GetComponent<Text>();
        comboText.text = "x " + comboMultiplier;
        
    }

    public void OnTurnOver(List<Chain> chains)
    {
        List<Chain> comboChains = chains.Where(c => c.chainColor == nextCombo).ToList();
        HashSet<GameObject> blocksToDestroy = new HashSet<GameObject>();

        if (comboChains.Count > 0)
        {
            
            foreach(Chain chain in comboChains)
            {
                // todo update score
                blocksToDestroy.UnionWith(chain.blocks);
            }
            comboMultiplier += COMBO_INCREMENT;

            if(nextCombo == "White")
            {
                SetNextComboColor("Black");
            } else
            {
                SetNextComboColor("White");
            }

            gameBoard.DestroyBlocks(blocksToDestroy);

        } else
        {
            foreach(Chain chain in chains)
            {
                // todo update score
                blocksToDestroy.UnionWith(chain.blocks);
            }
            comboMultiplier = 1f;
            gameBoard.DestroyBlocks(blocksToDestroy);
        }
    }

    private void SetNextComboColor(string nextColor)
    {
        nextCombo = nextColor;
        Image comboColor = GameObject.Find("ComboColor").GetComponent<Image>();
        if(nextCombo == "White")
        {
            comboColor.color = Color.white;
        } else if(nextCombo == "Black")
        {
            comboColor.color = Color.black;
        }
        
    }
}
