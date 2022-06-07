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
    public const int scorePerOrder = 5000;
    public const int scorePerColor = 1000;
    public const int starNum = 3;
}

public class GameManager : MonoBehaviour
{
    public bool gameAlive = false; // Is the game currently running
    public int[] colorKey = new int[Constants.Normal]; // Key to win the game
    private int[] userKey = new int[Constants.Normal]; // Key the player is guessing
    public int numCorrectColor; // Number of colors guesses correctly that are currently inside of the key
    public int numCorrectOrder; // Number of colors guesses that are both the same color and the right position
    public int numToWin = Constants.Normal; // Number of colors needed to get correct in correct order to win
    public bool gameWon; // Bool for if player has won the game
    public double posKeyInput; // Position of key answer panel input
    public GameObject keyOptionsPanel; // Key option gameobject
    public int boxNumberEnterring; // Number of the blank box user is inputting into
    public int colorKeyNumberEnterring; // Number of the color user is inputting into blank box
    public Material blankMaterial; // Blank material for key input
    public List<int> numDifColors = new List<int>(); // Number of different colors inside original key
    public List<int> correctUniqueColor = new List<int>(); // List of correct unique colors
    private List<GameObject> pegs = new List<GameObject>(); // List of pegs in game
    public int attempts; // Number of attempts done by the user
    public GameObject restartButton; // Restart button for game
    public GameObject nextLvlButton; // Button to proceed to the next level
    public int attemptsAllowed = 5; // Attempts allowed by user
    private ScoreScript score;

    public TMP_Text introText; // Intro text for the game
    public TMP_Text gameText; // Game feedback text

    public ParticleSystem fireworkSystem; // Firework particle system

    private GameObject[] inputBoxesHistory = new GameObject[Constants.Normal]; // Array for history of answers given
    private GameObject[] inputBoxes = new GameObject[Constants.Normal]; // Specific history of answer given

    private GameObject[] answerPanelHistory; // History for answer panel for the key inputs
    public GameObject answerHistory; // Parent object holding all of the answers

    public GameObject blackPeg; // Black pegs used to represent the number of colors in correct order
    public GameObject whitePeg; // White pegs used to represent the number of colors guessed correctly

    public bool sameAttempt = false; // Boolean to check if current attempt is the same as the previous one
    private int[] prevKey;

    // Start is called before the first frame update
    void Start()
    {
        introText = GameObject.Find("IntroMessage").GetComponent<TMP_Text>();
        gameText = GameObject.Find("InfoMessage").GetComponent<TMP_Text>();
        score = GameObject.Find("Score").GetComponent<ScoreScript>();

        fireworkSystem.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        startGame();
        enterKeys();
    }

    void startGame()
    {
        int i;

        if (Input.anyKeyDown && !gameAlive)
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
                colorKey[i] = Random.Range(0, Constants.Normal); // Every number will represent a color
            }

            for (i = 0; i < colorKey.Length; ++i)
            {
                if (!numDifColors.Contains(colorKey[i]))
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

        int[] initialKey = new int[Constants.Normal];
        
        attempts++;

        if(attempts != 1)
        {
            sameAttempt = prevKey.SequenceEqual(userKey);
        }

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

            for (i = 0; i < colorKey.Length; i++)
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

            foreach (int a in res)
            {
                numCorrectColor++;
            }

            correctUniqueColor.Clear();

            while (numCorrectColor > numDifColors.Count)
            {
                numCorrectColor--;
            }

            answerPanelHistory = GameObject.FindGameObjectsWithTag("HistoryPanel");

            foreach (GameObject historyPanel in answerPanelHistory)
            {
                historyPanel.transform.Translate(0, 1.5f, 0);

                if (historyPanel.transform.position.y >= 4.75)
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

            foreach (GameObject peg in GameObject.FindGameObjectsWithTag("Peg"))
            {
                peg.transform.Translate(0, 1.5f, 0);

                if (peg.transform.position.y >= 4.75)
                {
                    Destroy(peg);
                }
            }

            updateBlackPegs();
            updateWhitePegs();
            score.updateScore();
            StartCoroutine(displayText());

            for (i = 0; i < colorKey.Length; i++)
            {
                initialKey[i] = colorKey[i];
            }

            prevKey = userKey;
        }
    }

    IEnumerator displayText()
    {
        if (numCorrectColor == 0)
        {
            gameText.color = Color.red;
        }
        else
        {
            gameText.color = Color.green;
        }

        if (gameAlive && numCorrectOrder == numToWin && attempts <= attemptsAllowed)
        {
            gameAlive = false;
            gameWon = true;

            fireworkSystem.Play();
            gameText.text = $"You Won!";
            restartButton.SetActive(true);
            nextLvlButton.SetActive(true);
            yield return new WaitForSecondsRealtime(3);
        }

        if (gameAlive && attempts > attemptsAllowed)
        {
            gameAlive = false;
            gameText.color = Color.red;
            gameText.text = $"Game Over!";
            restartButton.SetActive(true);
            yield return new WaitForSecondsRealtime(3);
        }

        if (!gameAlive)
        {
            yield return new WaitForSecondsRealtime(3);
        }

        gameText.text = $"Number of colors correct: {numCorrectColor}";
        yield return new WaitForSecondsRealtime(3);

        if (numCorrectOrder == 0)
        {
            gameText.color = Color.red;
        }
        else if (numCorrectOrder == 1)
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

    void updateBlackPegs()
    {
        float tempX;
        float tempY;

        for (int i = 0; i < numCorrectColor; i++)
        {
            if (i % 2 != 0)
            {
                tempX = -.5f;
            }
            else
            {
                tempX = 0;
            }

            if (i > 1)
            {
                tempY = .5f;
            }
            else
            {
                tempY = 0;
            }

            Instantiate(blackPeg, new Vector2(blackPeg.transform.position.x + tempX, blackPeg.transform.position.y + tempY), blackPeg.transform.rotation);
        }


    }

    void updateWhitePegs()
    {
        float tempX;
        float tempY;

        for (int i = 0; i < numCorrectOrder; i++)
        {
            if (i % 2 != 0)
            {
                tempX = .5f;
            }
            else
            {
                tempX = 0;
            }

            if (i > 1)
            {
                tempY = .5f;
            }
            else
            {
                tempY = 0;
            }

            Instantiate(whitePeg, new Vector2(whitePeg.transform.position.x + tempX, whitePeg.transform.position.y + tempY), whitePeg.transform.rotation);
        }
    }
}