using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cage : MonoBehaviour
{
    public void Break() {
        Destroy(transform.parent.parent.gameObject);
    }
}
