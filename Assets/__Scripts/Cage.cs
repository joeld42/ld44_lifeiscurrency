using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cage : MonoBehaviour
{
    public void Break() {
        Destroy(GetComponentInParent<CageRoot>().gameObject);
    }
}
