using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using TMPro;

public class UIController : MonoBehaviour
{
    public PigController heroPig;

    [Header("Child Controls")]
    public RectTransform gameOverPanel;
    public TextMeshProUGUI coinsCounter;
    public TextMeshProUGUI speedCounter;

    public Button btnPlayAgain;

    // Start is called before the first frame update
    void Start()
    {
        gameOverPanel.gameObject.SetActive(false);
        heroPig.OnCoinCountChanged += UpdateCoinText;
        UpdateCoinText(heroPig.coinCount);

        GameGlobals.OnGameOver += GameOver;
    }

    private void OnEnable()
    {
        //btnPlayAgain.Select();

        EventSystem evRef = EventSystem.current; 
        evRef.SetSelectedGameObject( btnPlayAgain.gameObject );
    }

    private void OnDestroy()
    {
        GameGlobals.OnGameOver -= GameOver;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetButtonDown("Fire3") && (GameGlobals.instance.isGameOver))
        //{
        //    onPlayAgain();
        //}
        speedCounter.text = string.Format("Speed: ({0}, {1})", heroPig.Velocity.x.ToString("F2"), heroPig.Velocity.y.ToString("F2"));
    }

    void UpdateCoinText( int coins )
    {
        coinsCounter.text = string.Format("Coins: {0}", coins);
    }

    void GameOver()
    {
        gameOverPanel.gameObject.SetActive(true);
        //heroPig.gameObject.SetActive(false);
    }
    public void onPlayAgain()
    {
        GameGlobals.instance.RestartGame();
    }

    public void onBackToTitle()
    {
        SceneManager.LoadScene("TitleScreen");
    }

    public void onQuit()
    {
        Application.Quit();
    }
}
