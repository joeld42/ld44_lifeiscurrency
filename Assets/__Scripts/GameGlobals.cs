using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGlobals : MonoBehaviour
{
    public static GameGlobals instance;

    public delegate void GameOverAction();
    public static event GameOverAction OnGameOver;

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

    public void TriggerGameOver()
    {
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
