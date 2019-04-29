using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigAnimation : MonoBehaviour
{
    [Header("Eyes")]
    [SerializeField] private GameObject m_EyesOpen;
    [SerializeField] private GameObject m_EyesClosed;
    [SerializeField] private GameObject m_EyesSquint;
    [SerializeField] private GameObject m_EyesDead;

    [Header("Body")]
    [SerializeField] private GameObject m_Snout;
    [SerializeField] private GameObject m_LeftWing;
    [SerializeField] private GameObject m_RightWing;
    [SerializeField] private GameObject m_FrontLeftLeg;
    [SerializeField] private GameObject m_FrontRightLeg;
    [SerializeField] private GameObject m_BackLeftLeg;
    [SerializeField] private GameObject m_BackRightLeg;
    [SerializeField] private GameObject m_Tail;

    [Header("Items")]
    [SerializeField] private CoinVisualAnimation m_CoinVisualPrefab;
    [SerializeField] private TMPro.TextMeshPro m_CoinCounterText;

    enum EyeState
    {
        EyesOpen,
        EyesClosed,
        EyesSquint,
        EyesDead,
    }
    private EyeState m_EyeState;

    private float m_BlinkTimer;
    const float m_BlinkSpeed = 15.0f;
    const float m_BlinkProbability = 0.2f;

    private float m_SquintTimer;
    const float m_SquintSpeed = 1.0f;
    const float m_SquintProbability = 0.0f;

    private PigController m_PigController;
    private float m_WingFlapTimer;
    const float m_FlapSpeed = 5.0f;

    private float m_SnoutSniffleTimer;
    const float m_SniffleSpeed = 6.0f;
    const float m_SniffleProbability = 0.25f;

    const float m_WaggleSpeed = 2.0f;
    const float m_WaggleAmount = 20.0f;

    const float m_LegSwingRange = 60.0f;

    private int m_CoinCount;
    private List<CoinVisualAnimation> m_Coins;
    private float m_CoinCounterTimer;
    const float m_CoinCounterSpeed = 2.0f;
    private Vector3 m_CoinCounterInitialPosition;
    private Color m_CoinCounterInitialColor;

    // Start is called before the first frame update
    void Awake()
    {
        m_PigController = GetComponent<PigController>();
        GameGlobals.OnGameOver += GameOver;
        GameGlobals.OnGameRestart += GameRestart;
        m_PigController.OnCoinCountChanged += CoinCountChanged;
        m_Coins = new List<CoinVisualAnimation>();
        m_CoinCounterInitialPosition = m_CoinCounterText.rectTransform.localPosition;
        m_CoinCounterInitialColor = m_CoinCounterText.color;
    }

    private void OnDestroy()
    {
        GameGlobals.OnGameOver -= GameOver;
        GameGlobals.OnGameRestart -= GameRestart;
        m_PigController.OnCoinCountChanged -= CoinCountChanged;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            FlapWing();
            Sniffle();
        }

        if (m_WingFlapTimer > 0)
        {
            m_WingFlapTimer -= m_FlapSpeed * Time.deltaTime;
            if (m_WingFlapTimer < 0)
            {
                m_WingFlapTimer = 0;
            }
            float angle = 60.0f + 60.0f * (0.5f - 0.5f * Mathf.Cos(2.0f * Mathf.PI * m_WingFlapTimer));
            m_LeftWing.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            m_RightWing.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -angle));
        }

        if (m_SnoutSniffleTimer > 0)
        {
            m_SnoutSniffleTimer -= m_SniffleSpeed * Time.deltaTime;
            if (m_SnoutSniffleTimer < 0)
            {
                m_SnoutSniffleTimer = 0;
            }
            float angle = -10.0f * (0.5f - 0.5f * Mathf.Cos(2.0f * Mathf.PI * m_SnoutSniffleTimer));
            m_Snout.transform.localRotation = Quaternion.Euler(new Vector3(angle, -180.0f, 0));
        }
        else
        {
            if (Random.value > Mathf.Pow(1.0f - m_SniffleProbability, Time.deltaTime))
            {
                Sniffle();
            }
        }

        // tail waggle
        float theta = m_WaggleAmount * (Mathf.PerlinNoise(m_WaggleSpeed * Time.time, 0) + 0.5f * Mathf.PerlinNoise(2.01f * m_WaggleSpeed * Time.time, 0));
        float phi = m_WaggleAmount * (Mathf.PerlinNoise(m_WaggleSpeed * Time.time, 1) + 0.5f * Mathf.PerlinNoise(2.01f * m_WaggleSpeed * Time.time, 1));
        m_Tail.transform.localRotation = Quaternion.Euler(new Vector3(theta, phi, 0));

        // leg swing
        {
            float speed = m_PigController.Velocity.x;
            float angle = -m_LegSwingRange * (Mathf.SmoothStep(0, 1, 0.5f + 0.1f * speed) - 0.5f);
            m_FrontLeftLeg.transform.localRotation = Quaternion.Euler(new Vector3(angle, 0, 0));
            m_FrontRightLeg.transform.localRotation = Quaternion.Euler(new Vector3(angle, 0, 0));
            m_BackLeftLeg.transform.localRotation = Quaternion.Euler(new Vector3(angle, 0, 0));
            m_BackRightLeg.transform.localRotation = Quaternion.Euler(new Vector3(angle, 0, 0));
        }

        // eyes
        if (m_BlinkTimer > 0)
        {
            m_EyeState = EyeState.EyesClosed;
            m_BlinkTimer -= m_BlinkSpeed * Time.deltaTime;
            if (m_BlinkTimer < 0)
            {
                m_BlinkTimer = 0;
                m_EyeState = EyeState.EyesOpen;
            }
        }
        else if (m_SquintTimer > 0)
        {
            m_EyeState = EyeState.EyesSquint;
            m_SquintTimer -= m_SquintSpeed * Time.deltaTime;
            if (m_SquintTimer < 0)
            {
                m_SquintTimer = 0;
                m_EyeState = EyeState.EyesOpen;
            }
        }
        else
        {
            if (Random.value > Mathf.Pow(1.0f - m_BlinkProbability, Time.deltaTime))
            {
                Blink();
            }
            else if (Random.value > Mathf.Pow(1.0f - m_SquintProbability, Time.deltaTime))
            {
                Squint();
            }
        }
        m_EyesOpen.SetActive(m_EyeState == EyeState.EyesOpen);
        m_EyesClosed.SetActive(m_EyeState == EyeState.EyesClosed);
        m_EyesSquint.SetActive(m_EyeState == EyeState.EyesSquint);
        m_EyesDead.SetActive(m_EyeState == EyeState.EyesDead);

        // coin counter text
        m_CoinCounterText.transform.rotation = Quaternion.identity;
        if (m_CoinCount <= 0)
        {
            m_CoinCounterText.rectTransform.localPosition = m_CoinCounterInitialPosition;
            m_CoinCounterText.color = Color.red;
        }
        else if (m_CoinCount <= 3)
        {
            m_CoinCounterText.rectTransform.localPosition = m_CoinCounterInitialPosition;

            float dangerFactor = (4.0f - m_CoinCount) / 3.0f;
            float flashSpeed = 4.0f * dangerFactor;
            m_CoinCounterText.color = Color.Lerp(
                m_CoinCounterInitialColor,
                new Color(1.0f, 0.0f, 0.0f,
                    Mathf.Sin(flashSpeed * 2.0f * Mathf.PI * Time.time)),
                dangerFactor);
        }
        else if (m_CoinCounterTimer > 0)
        {

            m_CoinCounterTimer -= m_CoinCounterSpeed * Time.deltaTime;
            if (m_CoinCounterTimer < 0)
            {
                m_CoinCounterTimer = 0;
            }

            Color textColor = m_CoinCounterInitialColor;
            textColor.a = Mathf.SmoothStep(0.0f, 1.0f, m_CoinCounterTimer);
            m_CoinCounterText.color = textColor;
            m_CoinCounterText.rectTransform.localPosition = m_CoinCounterInitialPosition +
                1.0f * (1.0f - m_CoinCounterTimer) * (m_CoinCounterText.transform.localRotation * Vector3.up);
        }
    }

    public void Squint()
    {
        if (m_EyeState != EyeState.EyesDead)
        {
            m_BlinkTimer = 0.0f;
            m_SquintTimer = 1.0f;
        }
    }

    public void Blink()
    {
        if (m_EyeState != EyeState.EyesDead)
        {
            m_SquintTimer = 0.0f;
            m_BlinkTimer = 1.0f;
        }
    }

    void FlapWing()
    {
        m_WingFlapTimer = 1.0f;
    }

    void Sniffle()
    {
        m_SnoutSniffleTimer = 1.0f;
    }

    void GameOver()
    {
        Debug.Log("Game OVER");
        m_EyeState = EyeState.EyesDead;
        m_BlinkTimer = 0.0f;
        m_SquintTimer = 0.0f;
    }

    void GameRestart()
    {
        Debug.Log("Game RESTART");
        m_EyeState = EyeState.EyesOpen;
        m_BlinkTimer = 0.0f;
        m_SquintTimer = 0.0f;
    }

    // This function assumes that the coins are transferred to a different
    // owner so doesn't do the visual animation of the coins popping off
    // the pile
    public void TransferCoins(int coinTransferCount)
    {
        for (int i = 0; i < coinTransferCount; i++)
        {
            m_CoinCount--;
            GameObject coinObject = m_Coins[m_CoinCount].gameObject;
            coinObject.SetActive(false);
            m_Coins.RemoveAt(m_CoinCount);
            Object.Destroy(coinObject);
        }
        m_CoinCounterTimer = 1.0f;
        m_CoinCounterText.text = m_CoinCount.ToString();
    }

    void CoinCountChanged(int coinCountTarget)
    {
        if (coinCountTarget > m_CoinCount)
        {
            // add coins
            while (m_CoinCount < coinCountTarget)
            {
                CoinVisualAnimation coin = Instantiate(m_CoinVisualPrefab, transform);
                Vector3 pilePosition = Mathf.Pow(.008f * m_CoinCount, .2f) * Random.onUnitSphere;
                pilePosition.y = 0.8f * Mathf.Abs(pilePosition.y);
                coin.transform.localPosition = new Vector3(0, .75f, 0);
                coin.SetTargetPosition(new Vector3(0, 0.75f, 0) + pilePosition);

                m_Coins.Add(coin);
                m_CoinCount++;
            }
            m_CoinCounterTimer = 1.0f;
        }
        else if (coinCountTarget < m_CoinCount)
        {
            // remove coins
            while (m_CoinCount > coinCountTarget && m_CoinCount > 0)
            {
                m_CoinCount--;
                m_Coins[m_CoinCount].Detach(m_PigController.Velocity);
                m_Coins.RemoveAt(m_CoinCount);
            }
            m_CoinCounterTimer = 1.0f;
        }

        m_CoinCounterText.text = m_CoinCount.ToString();
    }
}
