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
    public Vector2 upforce = new Vector2(0,400);
    public Vector2 sideforce = new Vector2(40,0);
    public Vector2 speedLimit = new Vector2(2,4);

    public float minMass = 6f, coinMass = 0.5f;

    [Header("Buoyancy")]
    public float airDensity = 1;
    public float waterDensity = 1000;
    [SerializeField]
    private Vector2 buoyancyForce, fullySubmergedForce, fullyEmergedForce;
    
    //[SerializeField]
    float volume;
    public float waterHeight = 0;
    //[SerializeField]
    bool inWater, fullySubmerged;

    // TODO: Replace with a fancy coin-meter 
    [Header("Coins")]
    public int coinCount = 20;

    [Header("Firing")]
    public float fireCoolDown;
    public Rigidbody2D bulletPrefab;

    public ForceMode2D forceMode;

    new private CircleCollider2D collider;
    public System.Action<int> CoinCountChanged;
    public System.Action<Vector3> CollidedAt;
    
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<CircleCollider2D>();
        RecalculateMass();

    }
    void Update()
    {

        fireCoolDown -= Time.deltaTime;

        if (Input.GetButtonDown("Jump"))
        {
            rigidbody.AddForce(upforce * minMass, forceMode);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            FireCoin();
        }

        rigidbody.AddForce(sideforce * minMass * Input.GetAxis("Horizontal"), forceMode);

        // Check if we fell off the world
        if (transform.position.y < -5.0f)
        {
            GameGlobals.instance.TriggerGameOver();
        }

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
        RecalculateBuoyancy();
        rigidbody.AddForce(buoyancyForce);
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
        RecalculateMass();
        if (CoinCountChanged != null) CoinCountChanged(coinCount);
        if (coinCount <=0)
        {
            GameGlobals.instance.TriggerGameOver();
        }
    }

    void RecalculateMass()
    {
        rigidbody.mass = minMass + coinCount * coinMass;
        float radius = collider.radius;
        volume = 4f * Mathf.PI * radius * radius * radius / 3f;
        fullySubmergedForce = volume * waterDensity * -Physics.gravity;
        fullyEmergedForce = volume * airDensity * -Physics.gravity;

        RecalculateBuoyancy();
    }

    void RecalculateBuoyancy()
    {
        float radius = collider.radius;
        float h = waterHeight + radius - transform.position.y;

        inWater = h > 0;
        fullySubmerged = h > radius * 2;
            
        if (!inWater)
        {
            buoyancyForce = fullyEmergedForce;
        } 
        else if (fullySubmerged)
        {
            buoyancyForce = fullySubmergedForce;            
        } else {

            float asq = 2 * radius * h - h * h;
            float submergedVolume = ((Mathf.PI * h)/ 6.0f)*(3*asq+h*h);

            buoyancyForce = submergedVolume * waterDensity * -(Vector2)Physics.gravity;
            buoyancyForce += (volume - submergedVolume) * airDensity * -(Vector2)Physics.gravity;
	   }

        // if (debugDraw) Debug.DrawRay(wp, force/mass);
    }
    
}
