using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PA.HealthSystem;//health sistemi ile ilgili referans

//enemy sýnýfý, düþman karakterinin davranýþlarýný tanýmlar
public class Enemy : MonoBehaviour
{
    public Player player;//oyuncu karakterine referans
    [SerializeField]
    private int initialHealthValue = 3;//düþmanýn baþlangýç saglýk degeri

    public GameObject projectile;//düþmanýn ateþledigi mermi objesi
    public float shootingDelay;//ateþ etme gecikmesi

    public bool isShooting = false;//düþmanýn þu anda ateþ edip etmedigini kontrol eden degisken 

    public float speed = 2;//düþmanýn hareket hýzý 
    public float speedVariation = 0.3f;//hareket hýzýna rastgele varyasyýn ekler
    private Rigidbody2D rb2d;//düþmanýn fizik bileþeni
    bool firstShoot = true;//ilk ateþin rastgele bir gecikmeyle yapýlmasýný saglar 

    public EnemySpawner enemySpawner;// düþmanýn yaratan spawner'ý referans 


    [SerializeField] private Health health;//düþmanýn saglýk sistemi referansý

    //oyun baþladýgýnda nesne olusturulurken ilk cagrýlan fonksiyon
    private void Awake()
    {
        health = GetComponent<Health>(); ;//düþmanýn saglýk bileþenini alýr
        player = FindObjectOfType<Player>();// oyuncu nesnesini alýr
        rb2d = GetComponent<Rigidbody2D>();// düþmanýn rigidbody2D bileþenini alýr
        speed += UnityEngine.Random.Range(0, speedVariation);// hýza rastgele bir varyasyon ekler
    }
    // oyun baþladýgýnda çagrýlan fonksiyon
    private void Start()
    {
        health.InitializeHealth(initialHealthValue);//düþmanýn saglýgýný baþlatýr
        
    }
    //her karede çagrýlan fonksiyon
    private void Update()
    {
        //Vector3 movementDirection = Vector3.down * speed * Time.deltaTime;
        //transform.Translate(movementDirection, Space.World);

        //eger oyuncu hayattaysa 
        if (player.isAlive)
        {
            Vector3 desiredDirection = player.transform.position - transform.position;//oyuncuya dogru yönelir
            float desiredAngle = Mathf.Atan2(desiredDirection.y, desiredDirection.x) * Mathf.Rad2Deg - 90;//dönüþ açýsýný hesaplar
            transform.rotation = Quaternion.AngleAxis(desiredAngle, Vector3.forward);// düþmaný oyuncuya döndürür

            //düþman ateþ etmiyorsa, ateþ etmeye baþla
            if (isShooting == false)
            {
                isShooting = true;//ateþ emte durumunu aktif eder
                StartCoroutine(ShootWithDelay(shootingDelay));//belirtilen gecikmeyle ateþ etme iþlemi baþlatýlýr
            }
        }
    }
    // fiziksel güncellemeler (her karede çagrýlýr)
    private void FixedUpdate()
    {
        rb2d.MovePosition(rb2d.position + Vector2.down * speed * Time.deltaTime);//düþmaný assagý dogru hareket ettirir
    }
    //belirli bir gecikmeden sonra ateþ eden corotuine
    private IEnumerator ShootWithDelay(float shootingDelay)
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
    //düþman bir nesneyle çarðýþtýgýnda caðrýlýr
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);

        //eðer çarpýþan nesne oyuncuysa 
        if (collision.GetComponent<Player>())
        {
            IHittable hittable = collision.GetComponent<IHittable>();// çarpýþan nesnenin "hittable" olup olmadýgýný konrtol eder
            if (hittable != null && collision.GetComponent<Player>())// eðer hittable bir nesne ve oyuncuysa 
            {
                hittable.GetHit(1, gameObject);//oyuncuya hasar verir
                Death();
            }
        }

        //Debug.Log(collision.name);


    }
    // düþman oyun alanýný dýþýna çýktýgýnda çagrýlan fonksiyon
    public void EnemyKilledOutsideBounds()
    {
        enemySpawner.EnemyKilled(this, false);// düþmaný spawner'a bildirir
        Destroy(gameObject);// düþmaný sahneden kaldýrýr
    }



    //düþman öldügünde çagrýlan fonksiyon
    public void Death()
    {
        enemySpawner.EnemyKilled(this, true);// spawner'a düþmanýn öldügünü bildirir (oyuncu tarafýndan öldürülmüþse))
        StopAllCoroutines();//tüm coroutine iþlemlerini durdurur
        GetComponent<SpriteRenderer>().enabled = false;// düþmanýn sprite'ýný gizler
        Destroy(gameObject); // düþmaný sahneden yok eder
    }
}
