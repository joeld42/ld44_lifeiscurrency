using System.Collections;
using System.Collections.Generic;

using UnityEngine.SceneManagement;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

// [Requires("Rigidbody2D")]
public class PigController : MonoBehaviour
{
    new private Rigidbody2D rigidbody;
    public Vector2 upforce = new Vector2(0,200);
    public Vector2 sideforce = new Vector2(10,0);
    public Vector2 speedLimit = new Vector2(2,4);
   


    // TODO: Replace with a fancy coin-meter 
    [Header("Coins")]
    public TextMeshProUGUI coinsCounter;
    public int coinCount = 20;

    [Header("Firing")]
    public float fireCoolDown;
    public Rigidbody2D bulletPrefab;

    public ForceMode2D forceMode;

    public RectTransform gameOverPanel;
    
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        gameOverPanel.gameObject.SetActive(false);
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

        coinsCounter.text = string.Format("Coins: {0}", coinCount );
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Coin")
        {
            //collision.gameObject.SendMessage("ApplyDamage", 10);
            Debug.Log("OnCollisionEnter2D "+collision.gameObject);
            rigidbody.mass += 1;
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

    void GameOver()
    {
        gameOverPanel.gameObject.SetActive(true);

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

            ChangeCoinCoint(-1);
        }
    }

    void ChangeCoinCoint( int changeAmount )
    {
        coinCount = coinCount + changeAmount;
        if (coinCount <=0)
        {
            GameOver();
        }
    }
    
}
