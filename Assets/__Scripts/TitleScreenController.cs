using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       // if (Input.GetButtonDown("Fire1"))
       // {
       //    OnPlayPressed();
       // }
        
    }

    public void OnPlayPressed()
    {
        // Make sure to clear the game over flag
        if (GameGlobals.instance != null)
        {
            GameGlobals.instance.isGameOver = false;
            GameGlobals.instance.nextLevelToLoad = "";
        }
        SceneManager.LoadScene("PigGame");
    }

    public void OnPlayEndlessPressed()
    {
        // Make sure to clear the game over flag
        if (GameGlobals.instance != null)
        {
            GameGlobals.instance.isGameOver = false;
            GameGlobals.instance.nextLevelToLoad = "Level_GenRandom";
        }
        SceneManager.LoadScene("PigGame");
    }
}
