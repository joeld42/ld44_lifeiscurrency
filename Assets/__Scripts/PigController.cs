using System.Collections;
using System.Collections.Generic;

using UnityEngine.SceneManagement;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

// [Requires("Rigidbody2D")]
public class PigController : MonoBehaviour
{
    [Header("Movement")]

    new private Rigidbody2D rigidbody;
    public Vector2 upforce = new Vector2(0,200);
    public Vector2 sideforce = new Vector2(10,0);
    public Vector2 speedLimit = new Vector2(2,4);
   


    // TODO: Replace with a fancy coin-meter 
    [Header("Coins")]
    public int coinCount = 20;

    [Header("Firing")]
    public float fireCoolDown;
    public Rigidbody2D bulletPrefab;

    public ForceMode2D forceMode;


    public System.Action<int> CoinCountChanged;
    public System.Action<Vector3> CollidedAt;
    
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }
    void Update()
    {

        fireCoolDown -= Time.deltaTime;

        if (Input.GetButtonDown("Jump"))
        {
            rigidbody.AddForce(upforce, forceMode);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            FireCoin();
        }

        rigidbody.AddForce(sideforce * Input.GetAxis("Horizontal"), forceMode);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Coin")
        {
            IChangeScore coin = other.gameObject.GetComponent<IChangeScore>();
            if (coin == null) {
                Debug.LogError("Collided without IChangeScore tagged as Coin "+other.gameObject.name, other.gameObject);
                return;
            }
            coin.ChangeScore(this);
        }
    }
    void FixedUpdate()
    {
        Vector2 v = rigidbody.velocity;
        if (Mathf.Abs(v.x) > speedLimit.x) v.x = speedLimit.x * Mathf.Sign(v.x);
        if (Mathf.Abs(v.y) > speedLimit.y) v.y = speedLimit.x * Mathf.Sign(v.y);
    }

    public void onPlayAgain()
    {
        SceneManager.LoadScene("PigGame");
    }

    void FireCoin()
    {
        //if (fireCoolDown > 0) return;
        if (coinCount > 0)
        {
            //use screen forward, not rigidbody til we get spinning working
            Vector3 p = transform.position + new Vector3(1.2f, 0, 0);
            Rigidbody2D bullet = Instantiate(bulletPrefab, p, Quaternion.identity);
            // Rigidbody2D r2 = bullet.GetComponent<Rigidbody2D>;
            bullet.velocity = rigidbody.velocity + new Vector2(8, 0);
            fireCoolDown = 0.5f;

            ChangeCoinCount(-1);
        }
    }

    public void ChangeCoinCount( int changeAmount )
    {
        coinCount = coinCount + changeAmount;
        rigidbody.mass = Mathf.Max(coinCount/20f,1);

        if (CoinCountChanged != null) CoinCountChanged(coinCount);
        if (coinCount <=0)
        {
            GameGlobals.instance.TriggerGameOver();
        }
    }
    
}
