using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickBlankKey : MonoBehaviour
{
    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseDown()
    {
        // Distance between two blank inputs are 1.5 units on game scene
        // Make the options panel show up gradually rather than instantly (maybe use for with scale starting 0, going to 1)
        gameManager.posKeyInput = (((gameObject.transform.position.x / 1.5) + .5) * 165) - 85; // Position to open key input, gives -1, 0, 1 and 2 for the boxes respectively
        
    }

    
}
