using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Coin : MonoBehaviour, IChangeScore
{

    float rotateAmount = 0;
    Vector3 angles;

    void Start()
    {
        angles = transform.root.localEulerAngles;

        Collider2D collider = GetComponent<Collider2D>();
        if ( ! collider.isTrigger)
        {
            Debug.LogError("Forcing Collider to be a trigger", this);
        }
    }

    private void Update() {
        rotateAmount += Time.deltaTime; // (Mathf.Sin(Time.timeSinceLevelLoad * 10.0f) - .5f) * 4;
        var eu = new Vector3();
        eu.y = angles.y + rotateAmount * 200;
        transform.localEulerAngles = eu;
    }

    public void ChangeScore(PigController pig)
    {
        AudioPlayer.PlayClip(GetComponent<AudioSource>().clip, .8f);
        pig.ChangeCoinCount( 1 );
        Destroy(gameObject);
    }
}
