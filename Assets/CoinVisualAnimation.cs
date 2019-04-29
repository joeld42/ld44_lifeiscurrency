using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinVisualAnimation : MonoBehaviour
{
    private const float m_AttachDuration = 0.25f;
    private const float m_DetachDuration = 0.5f;
    private const float m_Gravity = -100.0f;
    private Vector3 m_Velocity;
    private float m_StartVelocityY;
    private Vector3 m_StartPosition;
    private Vector3 m_EndPosition;
    private Quaternion m_StartRotation;
    private Quaternion m_EndRotation;
    private float m_Age;
    private bool m_IsDetaching;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_IsDetaching)
        {
            if (m_Age < m_AttachDuration)
            {
                m_Age += Time.deltaTime;
                if (m_Age > m_AttachDuration)
                {
                    m_Age = m_AttachDuration;
                }
                Vector3 newLocalPosition = Vector3.Lerp(m_StartPosition, m_EndPosition, m_Age / m_AttachDuration);
                newLocalPosition.y = m_StartPosition.y + m_StartVelocityY * m_Age + 0.5f * m_Gravity * m_Age * m_Age;
                transform.localPosition = newLocalPosition;
                transform.localRotation = Quaternion.SlerpUnclamped(m_EndRotation, m_StartRotation,
                    4.0f * (1.0f - m_Age / m_AttachDuration));
            }
        } 
        else
        {
            m_Age += Time.deltaTime;
            if (m_Age > m_DetachDuration)
            {
                Destroy(gameObject);
                gameObject.SetActive(false);
            }
            m_Velocity += new Vector3(0, m_Gravity * Time.deltaTime, 0);
            transform.localPosition += m_Velocity * Time.deltaTime;
        }
    }

    public void Detach(Vector3 inheritedVelocity)
    {

        transform.SetParent(null, true);
        //transform.parent = null;
        m_Age = 0;
        m_IsDetaching = true;
        m_Velocity = inheritedVelocity + new Vector3(0, 20.0f, 0) + 10.0f * Random.onUnitSphere;
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        m_StartPosition = transform.localPosition;
        m_EndPosition = targetPosition;

        m_StartRotation = Random.rotation;
        m_EndRotation = transform.localRotation;

        m_StartVelocityY =
             (m_EndPosition.y - m_StartPosition.y - 0.5f * m_Gravity * m_AttachDuration * m_AttachDuration) /
             m_AttachDuration;
    }
}
