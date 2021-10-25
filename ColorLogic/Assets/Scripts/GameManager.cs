using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

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
    public List<int> numDifColors = new List<int>(); // Number of different colors inside original key
    private List<int> correctUniqueColor= new List<int>();

    public TMP_Text introText;
    public TMP_Text gameText;

    GameObject inputBox0;
    GameObject inputBox1;
    GameObject inputBox2;
    GameObject inputBox3;

    private GameObject[] inputBoxesHistory = new GameObject[4];
    private GameObject[] inputBoxes = new GameObject[4];

    private GameObject[] answerPanelHistory;
    public GameObject answerHistory;
    
    // Start is called before the first frame update
    void Start()
    {
        int i;

        for (i = 0; i < Constants.Normal; i++)
        {
            inputBoxes[i] = GameObject.Find($"Box{i}");
        }

        introText = GameObject.Find("IntroMessage").GetComponent<TMP_Text>();
        gameText = GameObject.Find("InfoMessage").GetComponent<TMP_Text>();

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
 
       
    }

    void startGame()
    {
        int i;

        if(Input.anyKeyDown && !gameAlive)
        {
            gameAlive = true;

            introText.enabled = false;

            keyOptionsPanel.transform.position = new Vector2((inputBoxes[1].transform.position.x + inputBoxes[2].transform.position.x) / 2, keyOptionsPanel.transform.position.y); 

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

            print("Your color key is: " + colorKey[0] + colorKey[1] + colorKey[2] + colorKey[3]);
        }
    }

    public void gameLogic()
    {
        int i;
        int j;

        int[] initialKey = new int[4];

        if (gameAlive && inputBox0.GetComponent<SpriteRenderer>().sharedMaterial != blankMaterial && inputBox1.GetComponent<SpriteRenderer>().sharedMaterial != blankMaterial && inputBox2.GetComponent<SpriteRenderer>().sharedMaterial != blankMaterial && inputBox3.GetComponent<SpriteRenderer>().sharedMaterial != blankMaterial)
        {
            print("User key is: " + userKey[0] + userKey[1] + userKey[2] + userKey[3]);

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

            if (numCorrectOrder == numToWin)
            {
                gameAlive = false;
                gameWon = true;

                gameText.text = $"You Won!";
            }

            for (i = 0; i < colorKey.Length; i++)
            {
                initialKey[i] = colorKey[i];
            }
        }

        
    }

    IEnumerator displayText()
    {
        gameText.text = $"Number of colors correct: {numCorrectColor}";
        yield return new WaitForSecondsRealtime(3);
        gameText.text = $"Number of colors in correct order: {numCorrectOrder}";
        yield return new WaitForSecondsRealtime(3);

        gameText.text = "";
    }

    void enterKeys()
    {
        userKey[boxNumberEnterring] = colorKeyNumberEnterring;
    }  
}
