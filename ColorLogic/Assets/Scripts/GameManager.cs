using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

static class Constants
{
    public const int Normal = 4;
}

public class GameManager : MonoBehaviour
{
    public bool gameAlive = false; // Is the game currently running
    public int[] colorKey = new int[Constants.Normal]; // Key to win the game
    public int[] userKey = new int[Constants.Normal]; // Key the player is guessing
    public int numCorrectColor; // Number of colors guesses correctly that are currently inside of the key
    public int numCorrectOrder; // Number of colors guesses that are both the same color and the right position
    private int numToWin = Constants.Normal;
    public bool gameWon;
    public double posKeyInput;
    public GameObject keyOptionsPanel;
    public int boxNumberEnterring; // Number of the blank box user is inputting into
    public int colorKeyNumberEnterring; // Number of the color user is inputting into blank box
    public Material blankMaterial;
    public List<int> numDifColors = new List<int>(); // Number of different colors inside original key
    public List<int> correctUniqueColor= new List<int>();
    public int attempts; // Number of attempts done by the user
    public GameObject restartButton;
    public int attemptsAllowed = 5; // Attempts allowed by user

    public TMP_Text introText;
    public TMP_Text gameText;

    public ParticleSystem fireworkSystem;

    private GameObject[] inputBoxesHistory = new GameObject[4];
    private GameObject[] inputBoxes = new GameObject[4];

    private GameObject[] answerPanelHistory;
    public GameObject answerHistory;

    public GameObject blackPeg;
    public GameObject whitePeg;

    // Start is called before the first frame update
    void Start()
    {
        introText = GameObject.Find("IntroMessage").GetComponent<TMP_Text>();
        gameText = GameObject.Find("InfoMessage").GetComponent<TMP_Text>();

        // runTimer = GameObject.Find("RunTimer");

        fireworkSystem.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        startGame();
        enterKeys();

        // runTimer.GetComponent<TMP_Text>().text = $"Timer: {runTime:0.00}"; 
    }

    void startGame()
    {
        int i;

        if(Input.anyKeyDown && !gameAlive)
        {
            gameAlive = true;

            introText.enabled = false;

            for (i = 0; i < Constants.Normal; i++)
            {
                inputBoxes[i] = GameObject.Find($"Box{i}");
            }

            attempts = 0;

            keyOptionsPanel.transform.position = new Vector2((inputBoxes[0].transform.position.x), keyOptionsPanel.transform.position.y); 

            keyOptionsPanel.SetActive(true);

            for (i = 0; i < colorKey.Length; i++)
            {
                colorKey[i] = Random.Range(0, 4); // Every number will represent a color
            }

            for (i = 0; i < colorKey.Length; ++i)
            {
                if(!numDifColors.Contains(colorKey[i]))
                {
                    numDifColors.Add(colorKey[i]);
                }
            }

            while (numDifColors.Count < Constants.Normal)
            {
                numDifColors.Add(-1); // -1 is just a filler to make sure the list size is the same size as the box amounts
            }

            //print("Your color key is: " + colorKey[0] + colorKey[1] + colorKey[2] + colorKey[3]);
        }
    }

    public void gameLogic()
    {
        int i;
        int j;
        int countBox = 0;

        attempts++;

        int[] initialKey = new int[4];

        foreach (GameObject box in inputBoxes)
        {
            if (box.GetComponent<SpriteRenderer>().sharedMaterial != blankMaterial)
            {
                countBox++;
            }
        }

        if (gameAlive && countBox == Constants.Normal)
        {

            //print("User key is: " + userKey[0] + userKey[1] + userKey[2] + userKey[3]);

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

            for (i = 0; i < numDifColors.Count; i++) // Checking if there is any correct color on any of the boxes
            {
                if (!correctUniqueColor.Contains(userKey[i]))
                {
                    correctUniqueColor.Add(userKey[i]);
                }
            }    

            IEnumerable<int> res = numDifColors.AsQueryable().Intersect(correctUniqueColor);

            foreach(int a in res)
            {
                numCorrectColor++;
            }

            correctUniqueColor.Clear();

            while (numCorrectColor> numDifColors.Count)
            {
                numCorrectColor--;
            }

            answerPanelHistory = GameObject.FindGameObjectsWithTag("HistoryPanel");

            foreach (GameObject historyPanel in answerPanelHistory)
            {
                historyPanel.transform.Translate(0, 1.5f, 0);

                if(historyPanel.transform.position.y >= 4.75)
                {
                    Destroy(historyPanel);
                }
            }

            for (i = 0; i < Constants.Normal; i++)
            {
                inputBoxesHistory[i] = Instantiate(inputBoxes[i], new Vector3(inputBoxes[i].transform.position.x + 9f, inputBoxes[i].transform.position.y - 1.5f), new Quaternion(0, 0, 0, 0));

                inputBoxesHistory[i].tag = "HistoryPanel";

                Destroy(inputBoxesHistory[i].GetComponent<OnClickBlankKey>());
            }

            for (i = 0; i < Constants.Normal; i++)
            {
                inputBoxes[i].GetComponent<SpriteRenderer>().material = blankMaterial;
            }

            StartCoroutine(displayText());

            for (i = 0; i < colorKey.Length; i++)
            {
                initialKey[i] = colorKey[i];
            }
        }
    }

    IEnumerator displayText()
    {

        if(numCorrectColor == 0)
        {
            gameText.color = Color.red;
        }
        else
        {
            gameText.color = Color.green;
        }

        if (gameAlive && attempts > attemptsAllowed)
        {
            gameAlive = false;
            gameText.color = Color.red;
            gameText.text = $"Game Over!";
            restartButton.SetActive(true);
            yield return new WaitForSecondsRealtime(3);
        }

        if (gameAlive && numCorrectOrder == numToWin && attempts <= attemptsAllowed)
        {
            gameAlive = false;
            gameWon = true;

            fireworkSystem.Play();

            gameText.text = $"You Won!";
            restartButton.SetActive(true);
            yield return new WaitForSecondsRealtime(3);
        }

        if (!gameAlive)
        {
            yield return new WaitForSecondsRealtime(3);
        }

        gameText.text = $"Number of colors correct: {numCorrectColor}";
        yield return new WaitForSecondsRealtime(3);

        if(numCorrectOrder == 0)
        {
            gameText.color = Color.red;
        }
        else if(numCorrectOrder == 1)
        {
            gameText.color = Color.yellow;
        }
        else
        {
            gameText.color = Color.green;
        }

        gameText.text = $"Number of colors in correct order: {numCorrectOrder}";
        yield return new WaitForSecondsRealtime(3);

        gameText.text = "";
    }

    void enterKeys()
    {
        userKey[boxNumberEnterring] = colorKeyNumberEnterring;
    }  

    public void restart(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    void updatePegs()
    {
        float tempX;
        float tempY;

        for(int i = 0; i < numCorrectOrder; i++)
        {
            if(i % 2 != 0)
            {
                tempX = .5f;
            }
            else
            {
                tempX = 0;
            }

            //if

            //Instantiate(blackPeg, new Vector2(blackPeg.transform.position.x + tempX, blackPeg.transform.position.y + tempY), blackPeg.transform.rotation);
        }
    }
}
