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
    private string nextCombo = null;

    private long score = 0;
    private int level = 1;
    private int secondsRemaining = 60 * 60;

    private GameBoard gameBoard;
    private bool isHandlingChains = false;

    private void Awake()
    {
        gameBoard = FindObjectOfType<GameBoard>();
        gameBoard.onChainsFound += HandleChains;
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
        if (nextCombo == "White")
        {
            comboColor.color = Color.white;
        }
        else if (nextCombo == "Black")
        {
            comboColor.color = Color.black;
        } else
        {
            comboColor.color = Color.grey;
        }
    }

    public void HandleChains(List<Chain> chains)
    {
        
        if (!isHandlingChains)
        {
            isHandlingChains = true;
            bool matchedCombo = chains.Any(c => c.chainColor == nextCombo);

            if(nextCombo == null && chains.Count > 0)
            {
                nextCombo = chains[0].chainColor;
            }
            else if (!matchedCombo)
            {
                comboMultiplier = 1f;
                nextCombo = null;
            }

        }

        if (chains.Count > 0)
        {
            ScoreAndDestroyChains(chains);
        }
        else
        {
            OnTurnOver();
        }

    }

    public void OnTurnOver()
    {
        if(nextCombo != null)
        {
            comboMultiplier += COMBO_INCREMENT;

            if (nextCombo == "White")
            {
                SetNextComboColor("Black");
            }
            else
            {
                SetNextComboColor("White");
            }
        }

        isHandlingChains = false;
        level = (int)Math.Max(1, Math.Ceiling(Math.Log10(score)));

    }

    private void SetNextComboColor(string nextColor)
    {
        nextCombo = nextColor;
    }

    private void ScoreAndDestroyChains(List<Chain> chains)
    {
        HashSet<GameObject> blocksToDestroy = new HashSet<GameObject>();

        foreach (Chain chain in chains)
        {
            ScoreChain(chain);
            blocksToDestroy.UnionWith(chain.blocks);
        }

        gameBoard.DestroyBlocks(blocksToDestroy);
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
