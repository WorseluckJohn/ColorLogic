using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

static class Constants
{
    public const int Normal = 4;
}

public class GameManager : MonoBehaviour
{
    public bool gameAlive = false; // Is the game currently running
    public int[] colorKey = new int[4]; // Key to win the game
    public int[] userKey = new int[4]; // Key the player is guessing
    private int numCorrectColor; // Number of colors guesses correctly that are currently inside of the key
    private int numCorrectOrder; // Number of colors guesses that are both the same color and the right position
    private int numToWin = Constants.Normal;
    public bool gameWon;
    public double posKeyInput;
    public GameObject keyOptionsPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        startGame();
        gameLogic(); 
    }

    void startGame()
    {
        int i;

        if(Input.anyKeyDown && !gameAlive)
        {
            gameAlive = true;

            for (i = 0; i < colorKey.Length; i++)
            {
                colorKey[i] = Random.Range(0, 4); // Every number will represent a color
                Debug.Log("Color key is: " + colorKey[i]);
            }
        }
    }

    void gameLogic()
    {
        int i;
        int j;

        int[] initialKey = colorKey;

        if(gameAlive)
        {
            for (i = 0; i < colorKey.Length; i++) // Checking if there is any correct color on any of the boxes
            {
                for (j = 0; j < colorKey.Length; j++)
                {
                    if (userKey[j] == colorKey[i])
                    {
                        colorKey[i] = 5;
                        numCorrectColor++;
                    }
                }
            }

            for (j = 0; i < colorKey.Length; i++) // Checking if these colors are in correct place
            {
                if (colorKey[j] == colorKey[j])
                {
                    numCorrectOrder++;
                }
            }
        }

        colorKey = initialKey;

        if(numCorrectOrder == numToWin)
        {
            gameAlive = false;
            gameWon = true;
        }
    }

    void gameMessage()
    {
       
    }

    void enterKeys()
    {
        // Move key options panel by 165 units for next option on canvas, left most option at -250
        // Every option will have 

        keyOptionsPanel.transform.position = new Vector2((float)posKeyInput, keyOptionsPanel.transform.position.y);

        double i;

        for (i = 0; i < 1; i = i + .05)
        {
            keyOptionsPanel.transform.localScale = new Vector2((float) i, (float) i);
        }

    }  

    
}
