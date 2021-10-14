using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool gameAlive = false; // Is the game currently running
    public int[] colorKey = new int[4]; // Key to win the game
    public int[] userKey = new int[4]; // Key the player is guessing
    private int numCorrectColor; // Number of colors guesses correctly that are currently inside of the key
    private int numCorrectOrder; // Number of colors guesses that are both the same color and the right position

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        startGame();
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

        if(gameAlive)
        {
            for(i = 0; i < colorKey.Length; i++) // Checking if there is any correct color
            {
                if(userKey[i] == colorKey[0])
                {

                }
            }

                  // Checking if these colors are in correct place
        }
    }
}
