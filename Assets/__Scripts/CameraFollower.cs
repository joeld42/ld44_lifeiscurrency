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
    
    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
        GameGlobals.OnGameRestart += GameRestart;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPos = camera.WorldToScreenPoint(target.position);

        if (screenPos.x < 0f && !GameGlobals.instance.isGameOver && !m_RestartingGame)
        {
            // Flew off the left
            GameGlobals.instance.TriggerGameOver();
        }

        float marginPx = Screen.width * margin;
        float screenEdge = Screen.width - marginPx;
        float moveAmt = screenPos.x - screenEdge;
        float minSpeed = GameGlobals.instance.isGameOver ? 0.0f : 2.0f;
        moveAmt = Mathf.Clamp(moveAmt, minSpeed, marginPx);
        camera.transform.position += Vector3.right * Time.deltaTime * moveAmt;

        if (m_RestartingGame)
        {
            m_RestartingGame = false;
        }
    }

    void GameRestart()
    {
        m_RestartingGame = true;
    }
}
