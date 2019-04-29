using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EndOfLevel : MonoBehaviour
{
    public string nextLevel;
    SceneryLoader loader;
    // Start is called before the first frame update
    void Start()
    {
        Collider2D collider = GetComponent<Collider2D>();
        if ( ! collider.isTrigger)
        {
            Debug.LogError("Forcing Collider to be a trigger", this);
        }

        loader = FindObjectOfType(typeof(SceneryLoader)) as SceneryLoader;
        if (loader == null)
        {
            Debug.LogError("Unable to find a SceneryLoader",this);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PigController>() != null)
        {
            Debug.Log("EndOfLevel by "+other.name, this);
            Debug.Log("Loading level: " + nextLevel);
            loader.LoadLevel(nextLevel);
        }
    }
}
