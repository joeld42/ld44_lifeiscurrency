using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimer : MonoBehaviour
{
    public float life = 2;

    // Update is called once per frame
    void Update()
    {
        //should be pooled
        life -= Time.deltaTime;
        if (life < 0) Destroy(gameObject);
        
    }
}
