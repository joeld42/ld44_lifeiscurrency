using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cage : MonoBehaviour
{
    public void Break() {
        var cageRoot = GetComponentInParent<CageRoot>();
        cageRoot?.Break();
    }
}
