﻿using System.Collections;
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
    }

    void FlapWing()
    {
        m_WingFlapTimer = 1.0f;
    }
}
