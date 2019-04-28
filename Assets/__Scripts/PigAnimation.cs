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
    const float m_SquintProbability = 0.1f;
    private float m_SquintTimer;
    const float m_SquintSpeed = 1.0f;

    private PigController m_PigController;
    private float m_WingFlapTimer;
    const float m_FlapSpeed = 5.0f;

    private float m_SnoutSniffleTimer;
    const float m_SniffleSpeed = 6.0f;
    const float m_SniffleProbability = 0.25f;

    const float m_WaggleSpeed = 2.0f;
    const float m_WaggleAmount = 20.0f;

    const float m_LegSwingRange = 60.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_PigController = GetComponent<PigController>();
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
    }

    public void Alive()
    {
        m_EyeState = EyeState.EyesOpen;
        m_BlinkTimer = 0.0f;
        m_SquintTimer = 0.0f;
    }

    public void Dead()
    {
        m_EyeState = EyeState.EyesDead;
        m_BlinkTimer = 0.0f;
        m_SquintTimer = 0.0f;
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
}
