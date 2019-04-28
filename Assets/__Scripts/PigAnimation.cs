using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigAnimation : MonoBehaviour
{
    [SerializeField] private GameObject m_Snout;
    [SerializeField] private GameObject m_LeftWing;
    [SerializeField] private GameObject m_RightWing;
    [SerializeField] private GameObject m_FrontLeftLeg;
    [SerializeField] private GameObject m_FrontRightLeg;
    [SerializeField] private GameObject m_BackLeftLeg;
    [SerializeField] private GameObject m_BackRightLeg;
    [SerializeField] private GameObject m_Tail;

    private float m_WingFlapTimer;
    const float m_FlapSpeed = 5.0f;

    private float m_SnoutSniffleTimer;
    const float m_SniffleSpeed = 6.0f;
    const float m_SniffleProbability = 0.25f;

    const float m_WaggleSpeed = 2.0f;
    const float m_WaggleAmount = 20.0f;

    private float m_ForwardSpeed;
    private float m_LastPosX;
    const float m_LegSwingRange = 30.0f;

    // Start is called before the first frame update
    void Start()
    {
        
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
            float pigPos = m_Snout.transform.position.x;
            float speed = (pigPos - m_LastPosX) / Time.deltaTime;
            m_LastPosX = pigPos;
            float angle = -m_LegSwingRange * Mathf.SmoothStep(0, 1, 0.5f + speed);
            m_FrontLeftLeg.transform.localRotation = Quaternion.Euler(new Vector3(angle, 0, 0));
            m_FrontRightLeg.transform.localRotation = Quaternion.Euler(new Vector3(angle, 0, 0));
            m_BackLeftLeg.transform.localRotation = Quaternion.Euler(new Vector3(angle, 0, 0));
            m_BackRightLeg.transform.localRotation = Quaternion.Euler(new Vector3(angle, 0, 0));
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
