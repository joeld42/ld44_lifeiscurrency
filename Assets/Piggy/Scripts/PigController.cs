using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[Requires("Rigidbody2D")]
public class PigController : MonoBehaviour
{
    new private Rigidbody2D rigidbody;
    public Vector2 upforce = new Vector2(0,200);
    public Vector2 sideforce = new Vector2(10,0);
    public Vector2 speedLimit = new Vector2(2,4);

    public ForceMode2D forceMode;
    
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            rigidbody.AddForce(upforce, forceMode);
        }

        // if (Input.GetAxis("Horizontal")  != 0)
        // {
            rigidbody.AddForce(sideforce * Input.GetAxis("Horizontal"), forceMode);
        // }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("OnCollisionEnter2D "+collision.gameObject);
        if (collision.gameObject.tag == "Coin")
        {
            //collision.gameObject.SendMessage("ApplyDamage", 10);
            rigidbody.mass += 1;
        }
    }

    void FixedUpdate()
    {
        Vector2 v = rigidbody.velocity;
        if (Mathf.Abs(v.x) > speedLimit.x) v.x = speedLimit.x * Mathf.Sign(v.x);
        if (Mathf.Abs(v.y) > speedLimit.y) v.y = speedLimit.x * Mathf.Sign(v.y);

    }
}
