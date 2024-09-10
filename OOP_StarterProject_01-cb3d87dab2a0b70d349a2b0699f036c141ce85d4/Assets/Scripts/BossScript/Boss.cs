using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PA.HealthSystem;
using Unity.VisualScripting;

public class Boss : MonoBehaviour
{
    public Player player;
    [SerializeField]  private int initialHealthValue = 500;
    

    public GameObject projectile;
    public float shootingDelay;

    public bool isShooting = false;

    public float speed = 1;
    public float speedVariation = 0.3f;
    private Rigidbody2D rb2d;
    bool firstShoot = true;

    public BossSpawner bossSpawner;

    [SerializeField] private Health health;

    private void Awake()
    {
        health = GetComponent<Health>(); 
        player = FindObjectOfType<Player>();
        rb2d = GetComponent<Rigidbody2D>();
        speed += UnityEngine.Random.Range(0, speedVariation);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Boss Kendisi: "+initialHealthValue);
        health.InitializeHealth(initialHealthValue);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isAlive)
        {
            Vector3 desiredDirection = player.transform.position - transform.position;
            float desiredAngle = Mathf.Atan2(desiredDirection.y, desiredDirection.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.AngleAxis(desiredAngle, Vector3.forward);

            if (isShooting == false)
            {
                isShooting = true;
                StartCoroutine(ShootingWithDelay(shootingDelay));
            }
        }
    }
    private void FixedUpdate()
    {
        rb2d.MovePosition(rb2d.position + Vector2.down * speed * Time.deltaTime);
    }

    private  IEnumerator ShootingWithDelay(float shootingDelay)
    {
        // ilk ateþte rastgele bir gecikme uygular
        if (firstShoot)
        {
            firstShoot = false;//ilk ateþin tamamlandýgýný belirtir.
            yield return new WaitForSeconds(UnityEngine.Random.Range(0, 0.5f));// rastgele bir gecikme bekler
        }
        yield return new WaitForSeconds(shootingDelay);// belirtilen gecikmeyi bekler
        GameObject p = Instantiate(projectile, transform.position, transform.rotation);//mermi nesnesini oluþturur

        isShooting = false;// ateþ etme iþlemi tamamlanýr, tekrar ateþ edilebilir
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>())
        {
            Debug.Log(collision.gameObject.name);
            IHittable hittable = collision.GetComponent<IHittable>();
            Debug.Log("can:" + initialHealthValue);
            if (hittable != null && collision.GetComponent<Player>())
            {
                hittable.GetHit(1, gameObject);
                Debug.Log("güncell:" + initialHealthValue );

                if (health.CurrentHealth <= 10) // Eðer saðlýk 0 veya daha düþükse boss ölür
                {
                    Debug.Log("güncell:" + initialHealthValue);

                    Death();
                }
            }
        }
    }
    public void BossKilledOutsideBounds()
    {
        bossSpawner.BossKilled(this, false);
        Destroy(gameObject);
    }

    public void Death()
    {
        bossSpawner.BossKilled(this, true);
        StopAllCoroutines();
        GetComponent<SpriteRenderer>().enabled = false;
        Destroy(gameObject);
    }
}
