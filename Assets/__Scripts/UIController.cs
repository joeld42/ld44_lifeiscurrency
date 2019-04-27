using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIController : MonoBehaviour
{
    public PigController heroPig;

    [Header("Child Controls")]
    public RectTransform gameOverPanel;
    public TextMeshProUGUI coinsCounter;

    // Start is called before the first frame update
    void Start()
    {
        gameOverPanel.gameObject.SetActive(false);
        heroPig.CoinCountChanged += UpdateCoinText;
        UpdateCoinText(heroPig.coinCount);

        GameGlobals.OnGameOver += GameOver;
    }

    private void OnDestroy()
    {
        GameGlobals.OnGameOver -= GameOver;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire3") && (GameGlobals.instance.isGameOver))
        {
            onPlayAgain();
        }
    }

    void UpdateCoinText( int coins )
    {
        coinsCounter.text = string.Format("Coins: {0}", coins);
    }

    void GameOver()
    {
        gameOverPanel.gameObject.SetActive(true);
    }
    public void onPlayAgain()
    {
        GameGlobals.instance.RestartGame();
    }
}
