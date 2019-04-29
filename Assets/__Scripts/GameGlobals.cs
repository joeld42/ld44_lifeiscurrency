using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameGlobals : MonoBehaviour
{
    public static GameGlobals instance;

    public delegate void GameOverAction();
    public static event GameOverAction OnGameOver;
    public delegate void GameRestartAction();
    public static event GameOverAction OnGameRestart;

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
        OnGameRestart?.Invoke();
    }

    public void TriggerGameOver()
    {
        Debug.Log("Triggering game over");
        isGameOver = true;
        OnGameOver?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
