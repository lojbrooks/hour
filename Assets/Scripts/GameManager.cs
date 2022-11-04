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
    private BlockType nextCombo = BlockType.Empty;
    private BlockType currentCombo = BlockType.Empty;

    private long score = 0;
    public static int level = 1;
    private int secondsRemaining = 60 * 60;
    private int timeMultiplier = 1;

    private GameBoard gameBoard;

    private void Awake()
    {
        gameBoard = FindObjectOfType<GameBoard>();
        gameBoard.onChainsFound += HandleChains;
        gameBoard.onPowerBlockDestroyed += HandlePowerBlockDestroyed;
        gameBoard.onTurnOver += OnTurnOver;
    }

    // Start is called before the first frame update
    void Start()
    {
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

        Image comboColor = GameObject.Find("ComboColor").GetComponent<Image>();
        if (nextCombo == BlockType.White)
        {
            comboColor.color = Color.white;
        }
        else if (nextCombo == BlockType.Black)
        {
            comboColor.color = Color.black;
        } else
        {
            comboColor.color = Color.grey;
        }
    }

    public void HandleChains(List<Chain> chains, bool isChaining)
    {
        if(!isChaining)
        {
            bool matchedCombo = chains.Any(c => c.chainColor == nextCombo);
            if(matchedCombo)
            {
                currentCombo = nextCombo;
            } else
            {
                currentCombo = chains[0].chainColor;
            }
        }

        ScoreChains(chains);
    }

    public void OnTurnOver(List<Block> powerBlocks)
    {

        ApplyPowerBlockPassiveEffects(powerBlocks);

        if(nextCombo != BlockType.Empty)
        {

            if(currentCombo == nextCombo)
            {
                comboMultiplier += COMBO_INCREMENT;
                if (nextCombo == BlockType.White)
                {
                    nextCombo = BlockType.Black;
                }
                else
                {
                    nextCombo = BlockType.White;
                }
            } else
            {
                nextCombo = BlockType.Empty;
                comboMultiplier = 1;
            }

            
        } else
        {
            comboMultiplier += COMBO_INCREMENT;

            if (currentCombo == BlockType.White)
            {
                nextCombo = BlockType.Black;
            }
            else
            {
                nextCombo = BlockType.White;
            }
        }

        currentCombo = BlockType.Empty;
        level = (int)Math.Max(1, Math.Ceiling(Math.Log10(score)));

    }


    private void ScoreChains(List<Chain> chains)
    {
        foreach (Chain chain in chains)
        {
            ScoreChain(chain);
        }
    }

    private void ScoreChain(Chain chain)
    {
        int chainSizeScore = chain.blocks.Count() - 2;
        score += (long) (level * chainSizeScore * comboMultiplier);
    }

    private void OnSecondPassed()
    {
        secondsRemaining -= (1 * timeMultiplier);
        if(secondsRemaining <= 0)
        {
            // todo game over
        }
    }

    private void HandlePowerBlockDestroyed(Block block)
    {
        
        switch(block.blockType)
        {
            case BlockType.Red:
                Debug.Log("Red destroyed");
                timeMultiplier++;
                break;
        }
    }

    private void ApplyPowerBlockPassiveEffects(List<Block> powerBlocks)
    {
        foreach(Block block in powerBlocks)
        {
            switch (block.blockType)
            {
                case BlockType.Red:
                    Debug.Log("Red in grid");
                    score += 2;
                    break;
            }
        }
    }

}
