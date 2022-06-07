using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreScript : MonoBehaviour
{
    public static ScoreScript Instance { get; private set; }

    public int highScore; 
    private GameManager gameManager;
    private TimeScript timer;
    public int score = 0;
    public TMP_Text scoreText;
    public TMP_Text hsText; // High score text
    public int attemptScore;
    public GameObject[] stars;
    public int[] starLimits = new int[Constants.starNum];

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        timer = GameObject.Find("Time").GetComponent<TimeScript>();
        scoreText = gameObject.GetComponent<TMP_Text>();
        hsText = GameObject.Find("HighScore").GetComponent<TMP_Text>();

        InvokeRepeating("decreaseScore", 1f, 1f);
        
        scoreText.SetText($"{score}");
    }

    // Update is called once per frame
    void Update()
    {
        saveHighScore();
        //giveStars();
    }
    
    public void updateScore()
    {
        if(gameManager.sameAttempt == false) // Checking if current attempt is the same attempt as the previous one
        {
            attemptScore = Constants.Normal / (gameManager.numDifColors.Count + 1 - gameManager.numCorrectColor) * 2500;
            attemptScore += gameManager.numCorrectOrder * 2500;
            attemptScore /= gameManager.attempts;
            
            score += attemptScore;
        }

        scoreText.SetText($"{score}");
    }

    void decreaseScore()
    {
        if (gameManager.gameAlive && score > 0)
        {
            score -= 10;
            scoreText.SetText($"{score}");
        }
    }

    void saveHighScore()
    {
        if(score > highScore && gameManager.gameWon)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }
    }

    void giveStars()
    {
        if(gameManager.gameWon)
        {
            for (int i = 0; i < starLimits.Length; i++)
            {
                if (score >= starLimits[i])
                {
                    stars[i].SetActive(true);
                    stars[i].gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
                }
            }

            hsText.SetText($"{highScore}");
        }
    }
}
