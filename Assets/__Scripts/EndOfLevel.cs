using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EndOfLevel : MonoBehaviour
{
    public string nextLevel;
    SceneryLoader loader;
    new Collider2D collider;
    // Start is called before the first frame update
    void Start()
    {
        loader = FindObjectOfType(typeof(SceneryLoader)) as SceneryLoader;
        if (loader == null)
        {
            Debug.LogError("Unable to find a SceneryLoader",this);
        }

        collider = GetComponent<Collider2D>();
        if ( ! collider.isTrigger)
        {
            Debug.LogError("Forcing Collider to be a trigger", this);
        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Triggered by "+other.name);
        loader.LoadLevel(nextLevel);
    }
}
