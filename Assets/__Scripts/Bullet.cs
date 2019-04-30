using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    private float m_StartTime;

    private void Start()
    {
        m_StartTime = Time.time;
    }

    private void Update()
    {
        if (Time.time - m_StartTime > 2.0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        var cage = other.GetComponent<Cage>();
        if (cage) {
            cage.Break();
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var pig = other.gameObject.GetComponent<PigController>();
        if (!pig)
        {
            Destroy(gameObject);
        }
    }
}
