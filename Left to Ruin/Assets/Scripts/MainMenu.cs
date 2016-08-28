// Date   : 28.08.2016 20:46
// Project: Left to Ruin
// Author : bradur

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    void Start () {
    
    }

    void Update () {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            StartGame();
        } else if (Input.GetKeyUp(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("game");
    }
}
