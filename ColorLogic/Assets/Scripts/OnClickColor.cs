using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickColor : MonoBehaviour
{
    public GameManager gameManager;
    public int colorKeyNumberEnterring;
    GameObject blankBox;
    public Material objectMaterial;

    // Start is called before the first frame update
    void Start()
    {
        gameManager.GetComponent<GameManager>();
        
        objectMaterial = gameObject.GetComponent<SpriteRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        blankBox = GameObject.Find($"Box{gameManager.boxNumberEnterring}");
    }

    private void OnMouseDown()
    {
        gameManager.colorKeyNumberEnterring = colorKeyNumberEnterring;
        
        blankBox.GetComponent<SpriteRenderer>().material = objectMaterial;
    }
}
