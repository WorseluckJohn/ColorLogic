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
    public int numCorrectColor; // Number of colors guesses correctly that are currently inside of the key
    public int numCorrectOrder; // Number of colors guesses that are both the same color and the right position
    private int numToWin = Constants.Normal;
    public bool gameWon;
    public double posKeyInput;
    public GameObject keyOptionsPanel;
    public int boxNumberEnterring; // Number of the blank box user is inputting into
    public int colorKeyNumberEnterring; // Number of the color user is inputting into blank box
    public Material blankMaterial;

    GameObject inputBox0;
    GameObject inputBox1;
    GameObject inputBox2;
    GameObject inputBox3;
    
    // Start is called before the first frame update
    void Start()
    {
        inputBox0 = GameObject.Find($"Box0");
        inputBox1 = GameObject.Find($"Box1");
        inputBox2 = GameObject.Find($"Box2");
        inputBox3 = GameObject.Find($"Box3");
    }

    // Update is called once per frame
    void Update()
    {
        startGame();
        enterKeys();
 
        /*if()
        {
            gameLogic();
        }
        */
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
            }

            print("Your color key is: " + colorKey[0] + colorKey[1] + colorKey[2] + colorKey[3]);
        }
    }

    public void gameLogic()
    {
        inputBox0.GetComponent<SpriteRenderer>().material = blankMaterial;
        inputBox1.GetComponent<SpriteRenderer>().material = blankMaterial;
        inputBox2.GetComponent<SpriteRenderer>().material = blankMaterial;
        inputBox3.GetComponent<SpriteRenderer>().material = blankMaterial;

        print("User key is: " + userKey[0] + userKey[1] + userKey[2] + userKey[3]);

        int i;
        int j;

        int[] initialKey = new int[4];

        if (gameAlive)
        {
            numCorrectColor = 0;
            numCorrectOrder = 0;

            for(i = 0; i < colorKey.Length; i++)
            {
               initialKey[i] = colorKey[i];
            }

            for (j = 0; j < colorKey.Length; j++) // Checking if these colors are in correct place
            {
                if (userKey[j] == colorKey[j])
                {
                    numCorrectOrder++;
                }
            }

            for (i = 0; i < colorKey.Length; i++) // Checking if there is any correct color on any of the boxes
            {
                for (j = 0; j < colorKey.Length; j++)
                {
                    if (userKey[j] == initialKey[i])
                    {
                        initialKey[i] = 5;
                        numCorrectColor++;
                    }
                }
            }

            if (numCorrectOrder == numToWin)
            {
                gameAlive = false;
                gameWon = true;
            }
        }

        

       
    }

    void gameMessage()
    {
       
    }

    void enterKeys()
    {
        userKey[boxNumberEnterring] = colorKeyNumberEnterring;

        
    }  
}
