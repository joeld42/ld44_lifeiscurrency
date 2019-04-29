using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform target;
    Camera camera;

    [Tooltip("Follow margin as % of screen size"), Range(0f, 1f)]
    public float margin = 0.25f;

    private bool m_RestartingGame;
    private float m_StartTime;
    
    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
        GameGlobals.OnGameRestart += GameRestart;
        m_StartTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_RestartingGame)
        {
            // delay one frame to give everyone else a chance to update
            m_RestartingGame = false;
            m_StartTime = Time.time;
        }
        else
        {
            Vector3 screenPos = camera.WorldToScreenPoint(target.position);

            if (screenPos.x < 0f && !GameGlobals.instance.isGameOver)
            {
                // Flew off the left
                GameGlobals.instance.TriggerGameOver();
            }

            float marginPx = Screen.width * margin;
            float screenEdge = Screen.width - marginPx;
            float moveAmt = screenPos.x - screenEdge;

            // ramp in min speed over the first few seconds of the game
            float minSpeed = GameGlobals.instance.isGameOver ? 0.0f : 2.0f;
            minSpeed *= Mathf.SmoothStep(0.0f, 1.0f, (Time.time - m_StartTime) / 3.0f - 1.0f);

            moveAmt = Mathf.Clamp(moveAmt, minSpeed, marginPx);
            camera.transform.position += Vector3.right * Time.deltaTime * moveAmt;
        }
    }

    void GameRestart()
    {
        m_RestartingGame = true;
    }
}
