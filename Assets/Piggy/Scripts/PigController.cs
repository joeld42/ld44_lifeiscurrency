using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [Requires("Rigidbody2D")]
public class PigController : MonoBehaviour
{
    new private Rigidbody2D rigidbody;
    public Vector2 upforce = new Vector2(0,200);
    public Vector2 sideforce = new Vector2(10,0);
    public Vector2 speedLimit = new Vector2(2,4);
    public Rigidbody2D bulletPrefab;

    public float fireCoolDown;

    public ForceMode2D forceMode;
    
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }
    void Update()
    {

        fireCoolDown -= Time.deltaTime;

        if (Input.GetButtonDown("Fire1"))
        {
            rigidbody.AddForce(upforce, forceMode);
        }

        if (Input.GetButtonDown("Fire2"))
        {
            FireCoin();
        }

        rigidbody.AddForce(sideforce * Input.GetAxis("Horizontal"), forceMode);
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

    void FireCoin()
    {
        //if (fireCoolDown > 0) return;
        //use screen forward, not rigidbody til we get spinning working
        Vector3 p = transform.position+new Vector3(1.2f, 0, 0);
        Rigidbody2D bullet = Instantiate(bulletPrefab, p, Quaternion.identity);
        // Rigidbody2D r2 = bullet.GetComponent<Rigidbody2D>;
        bullet.velocity = rigidbody.velocity + new Vector2(8,0);
        fireCoolDown = 0.5f;
    }
    
}
