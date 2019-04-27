using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageHelper : MonoBehaviour
{

    public HingeJoint anchor;
    public LineRenderer ropeLine;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ropeLine.positionCount = 3;
        ropeLine.SetPosition(0, anchor.anchor);

        Vector3 middle = (anchor.anchor + transform.position) * 0.5f;
        middle = middle + UnityEngine.Random.insideUnitSphere;
        ropeLine.SetPosition(1, middle );
        ropeLine.SetPosition(2, transform.position);
    }
}
