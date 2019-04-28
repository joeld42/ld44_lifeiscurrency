using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D other) {
        var cage = other.GetComponent<Cage>();
        if (cage) {
            cage.Break();
            Destroy(gameObject);
        }
    }
}
