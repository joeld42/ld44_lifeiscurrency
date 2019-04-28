using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public void Eat() {
        Destroy(transform.parent.gameObject);
    }
}
