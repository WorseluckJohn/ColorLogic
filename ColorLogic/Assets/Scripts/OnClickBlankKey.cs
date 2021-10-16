using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickBlankKey : MonoBehaviour
{
    public GameManager gameManager;
    float posToBeMoved;
    public GameObject keyOptions;
    public int boxNumberEnterring;

    // Start is called before the first frame update
    void Start()
    {
        gameManager.GetComponent<GameManager>();
        posToBeMoved = gameObject.transform.position.x;
 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseDown()
    {
        // Distance between two blank inputs are 1.5 units on game scene
        // Make the options panel show up gradually rather than instantly (maybe use for with scale starting 0, going to 1)
        keyOptions.transform.position = new Vector2(gameObject.transform.position.x, keyOptions.transform.position.y); // Position to open key input, gives -1, 0, 1 and 2 for the boxes respectively
        gameManager.boxNumberEnterring = boxNumberEnterring;
    }

    
}
