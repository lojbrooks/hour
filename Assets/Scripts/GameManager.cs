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

    private long score = 0;
    private int level = 1;
    private int secondsRemaining = 60 * 60;

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
        InvokeRepeating("OnSecondPassed", 1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        Text comboText = GameObject.Find("ComboText").GetComponent<Text>();
        comboText.text = "x " + comboMultiplier;


        Text scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        scoreText.text = score.ToString();

        Text levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Level " + level;

        Text timeText = GameObject.Find("TimeText").GetComponent<Text>();
        timeText.text = (secondsRemaining / 60).ToString("D2") + ":" + (secondsRemaining % 60).ToString("D2");
    }

    public void OnTurnOver(List<Chain> chains)
    {
        List<Chain> comboChains = chains.Where(c => c.chainColor == nextCombo).ToList();
        HashSet<GameObject> blocksToDestroy = new HashSet<GameObject>();

        if (comboChains.Count > 0)
        {
            
            foreach(Chain chain in comboChains)
            {
                ScoreChain(chain);
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
            comboMultiplier = 1f;

            foreach (Chain chain in chains)
            {
                ScoreChain(chain);
                blocksToDestroy.UnionWith(chain.blocks);
            }
          
            gameBoard.DestroyBlocks(blocksToDestroy);
        }

        
        level = (int) Math.Max(1, Math.Ceiling(Math.Log10(score)));
        
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

    private void ScoreChain(Chain chain)
    {
        int chainSizeScore = chain.blocks.Count() - 2;
        score += (long) (level * chainSizeScore * comboMultiplier);
    }

    private void OnSecondPassed()
    {
        if(--secondsRemaining <= 0)
        {
            // todo game over
        }
    }
}
