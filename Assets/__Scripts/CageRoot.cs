using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageRoot : MonoBehaviour
{
    float rotateAmount = 0;
    Vector3 angles;

    private void Start() {
        angles = transform.root.localEulerAngles;
    }

    void Update()
    {
        rotateAmount = (Mathf.Sin(Time.timeSinceLevelLoad * 1.0f) - .5f) * 4;
        var eu = new Vector3();
        eu.z = angles.z + rotateAmount;
        transform.localEulerAngles = eu;
    }
}
