using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameGlobals : MonoBehaviour
{
    public static GameGlobals instance;

    public delegate void GameOverAction();
    public static event GameOverAction OnGameOver;

    public bool isGameOver = false;

    void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            // The gameGlobals already exists
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void RestartGame()
    {
        isGameOver = false;
        SceneManager.LoadScene("PigGame");
    }

    public void TriggerGameOver()
    {
        isGameOver = true;
        if (OnGameOver != null)
        {
            OnGameOver();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
