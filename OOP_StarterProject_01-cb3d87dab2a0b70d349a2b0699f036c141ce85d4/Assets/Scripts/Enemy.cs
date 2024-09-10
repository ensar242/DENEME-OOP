using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PA.HealthSystem;//health sistemi ile ilgili referans

//enemy s�n�f�, d��man karakterinin davran��lar�n� tan�mlar
public class Enemy : MonoBehaviour
{
    public Player player;//oyuncu karakterine referans
    [SerializeField]
    private int initialHealthValue = 3;//d��man�n ba�lang�� sagl�k degeri

    public GameObject projectile;//d��man�n ate�ledigi mermi objesi
    public float shootingDelay;//ate� etme gecikmesi

    public bool isShooting = false;//d��man�n �u anda ate� edip etmedigini kontrol eden degisken 

    public float speed = 2;//d��man�n hareket h�z� 
    public float speedVariation = 0.3f;//hareket h�z�na rastgele varyasy�n ekler
    private Rigidbody2D rb2d;//d��man�n fizik bile�eni
    bool firstShoot = true;//ilk ate�in rastgele bir gecikmeyle yap�lmas�n� saglar 

    public EnemySpawner enemySpawner;// d��man�n yaratan spawner'� referans 


    [SerializeField] private Health health;//d��man�n sagl�k sistemi referans�

    //oyun ba�lad�g�nda nesne olusturulurken ilk cagr�lan fonksiyon
    private void Awake()
    {
        health = GetComponent<Health>(); ;//d��man�n sagl�k bile�enini al�r
        player = FindObjectOfType<Player>();// oyuncu nesnesini al�r
        rb2d = GetComponent<Rigidbody2D>();// d��man�n rigidbody2D bile�enini al�r
        speed += UnityEngine.Random.Range(0, speedVariation);// h�za rastgele bir varyasyon ekler
    }
    // oyun ba�lad�g�nda �agr�lan fonksiyon
    private void Start()
    {
        health.InitializeHealth(initialHealthValue);//d��man�n sagl�g�n� ba�lat�r
        
    }
    //her karede �agr�lan fonksiyon
    private void Update()
    {
        //Vector3 movementDirection = Vector3.down * speed * Time.deltaTime;
        //transform.Translate(movementDirection, Space.World);

        //eger oyuncu hayattaysa 
        if (player.isAlive)
        {
            Vector3 desiredDirection = player.transform.position - transform.position;//oyuncuya dogru y�nelir
            float desiredAngle = Mathf.Atan2(desiredDirection.y, desiredDirection.x) * Mathf.Rad2Deg - 90;//d�n�� a��s�n� hesaplar
            transform.rotation = Quaternion.AngleAxis(desiredAngle, Vector3.forward);// d��man� oyuncuya d�nd�r�r

            //d��man ate� etmiyorsa, ate� etmeye ba�la
            if (isShooting == false)
            {
                isShooting = true;//ate� emte durumunu aktif eder
                StartCoroutine(ShootWithDelay(shootingDelay));//belirtilen gecikmeyle ate� etme i�lemi ba�lat�l�r
            }
        }
    }
    // fiziksel g�ncellemeler (her karede �agr�l�r)
    private void FixedUpdate()
    {
        rb2d.MovePosition(rb2d.position + Vector2.down * speed * Time.deltaTime);//d��man� assag� dogru hareket ettirir
    }
    //belirli bir gecikmeden sonra ate� eden corotuine
    private IEnumerator ShootWithDelay(float shootingDelay)
    {
        // ilk ate�te rastgele bir gecikme uygular
        if (firstShoot)
        {
            firstShoot = false;//ilk ate�in tamamland�g�n� belirtir.
            yield return new WaitForSeconds(UnityEngine.Random.Range(0, 0.5f));// rastgele bir gecikme bekler
        }
        yield return new WaitForSeconds(shootingDelay);// belirtilen gecikmeyi bekler
        GameObject p = Instantiate(projectile, transform.position, transform.rotation);//mermi nesnesini olu�turur

        isShooting = false;// ate� etme i�lemi tamamlan�r, tekrar ate� edilebilir
    }
    //d��man bir nesneyle �ar���t�g�nda ca�r�l�r
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);

        //e�er �arp��an nesne oyuncuysa 
        if (collision.GetComponent<Player>())
        {
            IHittable hittable = collision.GetComponent<IHittable>();// �arp��an nesnenin "hittable" olup olmad�g�n� konrtol eder
            if (hittable != null && collision.GetComponent<Player>())// e�er hittable bir nesne ve oyuncuysa 
            {
                hittable.GetHit(1, gameObject);//oyuncuya hasar verir
                Death();
            }
        }

        //Debug.Log(collision.name);


    }
    // d��man oyun alan�n� d���na ��kt�g�nda �agr�lan fonksiyon
    public void EnemyKilledOutsideBounds()
    {
        enemySpawner.EnemyKilled(this, false);// d��man� spawner'a bildirir
        Destroy(gameObject);// d��man� sahneden kald�r�r
    }



    //d��man �ld�g�nde �agr�lan fonksiyon
    public void Death()
    {
        enemySpawner.EnemyKilled(this, true);// spawner'a d��man�n �ld�g�n� bildirir (oyuncu taraf�ndan �ld�r�lm��se))
        StopAllCoroutines();//t�m coroutine i�lemlerini durdurur
        GetComponent<SpriteRenderer>().enabled = false;// d��man�n sprite'�n� gizler
        Destroy(gameObject); // d��man� sahneden yok eder
    }
}
