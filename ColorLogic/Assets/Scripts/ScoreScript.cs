using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreScript : MonoBehaviour
{
    private GameManager gameManager;
    private TimeScript timer;
    public int score = 0;
    public TMP_Text scoreText;
    public int attemptScore;
    public int penaltyScore = 1000;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        timer = GameObject.Find("Time").GetComponent<TimeScript>();
        scoreText = gameObject.GetComponent<TMP_Text>();

        InvokeRepeating("decreaseScore", 1f, 1f);
        
        scoreText.SetText($"{score}");
    }

    // Update is called once per frame
    void Update()
    {

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
}
