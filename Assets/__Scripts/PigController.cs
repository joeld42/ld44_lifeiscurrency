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
    public AudioClip squealClip;
    public AudioClip wingFlap;
    public AudioClip grunt;

    public float rotationCorrectionSpeed = 0.5f;

    public float minMass = 6f, coinMass = 0.5f;

    [Header("Buoyancy")]
    public float airDensity = 1;
    public float waterDensity = 100;
    public float dragCoefficient = 0.995f;
    public float waterDragScale = 1;
    public float waterHeight = 0;

    [Header("Buoyancy Debug")]
    [SerializeField]
    float volume;
    [SerializeField]
    float area, density;
    [SerializeField]
    bool inWater, fullySubmerged;
    [SerializeField]
    private Vector2 buoyancyForce, fullySubmergedForce, fullyEmergedForce;


    private PigAnimation m_PigAnimation;
    private Vector3 m_InitialPosition;
    private Quaternion m_InitialRotation;

    // TODO: Replace with a fancy coin-meter 
    [Header("Coins")]
    public int coinCount = 20;

    [Header("Firing")]
    public float fireCoolDown;
    public Rigidbody2D bulletPrefab;

    public ForceMode2D forceMode;

    new private CircleCollider2D collider;
    public System.Action<int> OnCoinCountChanged;
    public System.Action<Vector3> OnCollidedAt;

    public float boost;
    public Vector2 Velocity => rigidbody.velocity;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<CircleCollider2D>();
        m_PigAnimation = GetComponent<PigAnimation>();
        m_InitialPosition = transform.localPosition;
        m_InitialRotation = transform.localRotation;
    }

    void Start()
    {
        Debug.Log("STARTING PIG AGAIN");
        RecalculateMass();
        OnCoinCountChanged?.Invoke(coinCount);
    }

    private void OnDestroy()
    {
    }

    void Update()
    {

        fireCoolDown -= Time.deltaTime;

        if (Input.GetButtonDown("Jump"))
        {
            AudioPlayer.PlayClip(wingFlap);
            rigidbody.AddForce(upforce * minMass, forceMode);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            FireCoin();
        }

        // Cheat
        if (Input.GetKeyDown("p"))
        {
            transform.position = new Vector3(177f, 1.3f, 0f);
        }

        float thrustDirection = Mathf.Sign(Input.GetAxis("Horizontal"));
        if (rigidbody.velocity.x * thrustDirection < speedLimit.x)
        {
            float thrustFactor = 1.0f - rigidbody.velocity.x * thrustDirection / speedLimit.x;
            rigidbody.AddForce(thrustFactor * sideforce * minMass * Input.GetAxis("Horizontal"), forceMode);
        }

        // Check if we fell off the world
        if (transform.position.y < -5.0f)
        {
            GameGlobals.instance.TriggerGameOver();
        }

        // drag the pig upright
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, 0.1f);

        // add drag
        rigidbody.velocity *= Mathf.Pow(dragCoefficient, Time.deltaTime * (inWater ? waterDensity : airDensity));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var cage = other.GetComponent<Cage>();
        if (cage) {
            var cageRoot = cage.GetComponentInParent<CageRoot>();
            if (cageRoot && cageRoot.gameObject.activeSelf)
            {
                m_PigAnimation.Squint();
                AudioPlayer.PlayClip(squealClip);
                cage.Break();
                ChangeCoinCount(-2);
            }
        }
        var fruit = other.GetComponent<Fruit>();
        if (fruit) {
            AudioPlayer.PlayClip(grunt);
            m_PigAnimation.Blink();
            fruit.Eat();
            rigidbody.AddForce(5.0f * sideforce * minMass, forceMode);
        }
        if (other.gameObject.tag == "Coin")
        {
            m_PigAnimation.Blink();

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
        CalculateForces();
        rigidbody.AddForce(buoyancyForce);
    }

    void LevelLoad()
    {
        transform.localPosition = m_InitialPosition;
        transform.localRotation = m_InitialRotation;
    }

    void FireCoin()
    {
        m_PigAnimation.Blink();

        //if (fireCoolDown > 0) return;
        if (coinCount > 0)
        {
            //use screen forward, not rigidbody til we get spinning working
            Vector3 p = transform.position + new Vector3(1.2f, 0, 0);
            Rigidbody2D bullet = Instantiate(bulletPrefab, p, Quaternion.identity);
            // Rigidbody2D r2 = bullet.GetComponent<Rigidbody2D>;
            //bullet.velocity = rigidbody.velocity + new Vector2(8, 0);
            bullet.velocity = new Vector2(8, 0);
            fireCoolDown = 0.5f;

            m_PigAnimation.TransferCoins(1);
            ChangeCoinCount(-1);
        }
    }

    public void ChangeCoinCount( int changeAmount )
    {
        coinCount = coinCount + changeAmount;
        RecalculateMass();
        OnCoinCountChanged?.Invoke(coinCount);
        if (coinCount <=0)
        {
            GameGlobals.instance.TriggerGameOver();
        }
    }

    void RecalculateMass()
    {
        rigidbody.mass = minMass + Mathf.Max(0.0f, coinCount) * coinMass;
        float radius = collider.radius;
        area = Mathf.PI * radius * radius;
        volume = 4f * area * radius / 3f;
        density = rigidbody.mass /volume;
        fullySubmergedForce = volume * waterDensity * -Physics.gravity;
        fullyEmergedForce = volume * airDensity * -Physics.gravity;

        CalculateForces();
    }

    void CalculateForces()
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
    }
    
}
