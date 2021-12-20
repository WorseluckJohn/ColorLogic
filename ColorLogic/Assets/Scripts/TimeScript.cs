using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;

public class TimeScript : MonoBehaviour
{
    private GameManager gameManager;
    public float time = 0;
    public TMP_Text timeText;

    // Start is called before the first frame update
    void Start()
    {
        timeText = gameObject.GetComponent<TMP_Text>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        timeText.SetText($"{time:0.00}");
    }

    // Update is called once per frame
    void Update()
    {
        timeUpdate();
    }

    void timeUpdate()
    {
        if(gameManager.gameAlive)
        {
            time += Time.deltaTime;
            timeText.SetText($"{time:0.00}");
        }
    }
}
