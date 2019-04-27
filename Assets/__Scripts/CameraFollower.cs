using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform target;
    Camera camera;

    [Tooltip("Follow margin as % of screen size"), Range(0f, 1f)]
    public float margin = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPos = camera.WorldToScreenPoint(target.position);
        float marginPx = Screen.width * margin;
        float screenEdge = Screen.width - marginPx;
        float moveAmt = screenPos.x - screenEdge;
        if (moveAmt > 0f)
        {
            moveAmt = Mathf.Clamp(moveAmt, 0.0f, marginPx);
            camera.transform.position += Vector3.right * Time.deltaTime * moveAmt;
        }

    }
}
