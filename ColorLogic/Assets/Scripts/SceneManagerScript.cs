using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    private Scene currScene;

    // Start is called before the first frame update
    void Start()
    {
        currScene = SceneManager.GetActiveScene();

    }

    public void restart()
    {
        SceneManager.LoadScene(currScene.name);
    }

    public void nextScene()
    {
        SceneManager.LoadScene(currScene.buildIndex + 1);
    }

}
