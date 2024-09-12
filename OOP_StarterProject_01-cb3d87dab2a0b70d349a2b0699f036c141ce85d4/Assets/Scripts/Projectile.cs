using PA.HealthSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//merminin davran��lar�n� y�netir.
public class Projectile : MonoBehaviour
{
    public float speed = 10;//merminin hareket h�z�
    public Rigidbody2D rb2d;// merminin fiziksel hareketini y�netmek i�in kullan�lan rigidbody2D bile�eni
    public float deathDelay = 5;//merminin sahnede ne kadar s�re aktif kalacag�n� belirleyen eleman 

    [SerializeField] private int initialHealth = 5;//merminin ba�lang�� sa�l�g� 
    [SerializeField] private Health health;//merminin sagl�g�n� y�netecek olan health bile�eni

    //public bool disabled = false;

    // Start is called before the first frame update

    //start metodu, mermi sahnede aktif oldugunda bir kez cagr�l�r
    void Start()
    {
        health = GetComponent<Health>();//merminin health bile�eni al�n�r ev ba�lang�� sagl�g� atan�r
        health.InitializeHealth(initialHealth);


        rb2d.velocity = transform.up * speed;//merminin hareket y�n� ve h�z� belirlenir.Transform'un "up" y�n�ne g�re h�z verilir
        StartCoroutine(DeathAfterDelay(deathDelay));//belirtilen bir s�re sonra merminin yok edilmesi i�in bir coroutine ba�lat�l�r.
    }

    private IEnumerator DeathAfterDelay(float deathDelay)//bu corutuine, belirli bir s�re bekledikten sonra mermiyi yok eder
    {
        yield return new WaitForSeconds(deathDelay);//belirtilen s�re kadar bekler
        health.GetHit(1,gameObject);//merminin bir hasar almas�n� simule eder
        Destroy(gameObject);//mermiyi yok eder
    }
    //mermi ba�ka bir collider ile carp�st�g�nda bu fonksiyon tetiklenir
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IHittable hittable = collision.GetComponent<IHittable>();//eger carp�lan nesne Ihittable aray�z�n� uygulay�p uygulamad�g�n� kontrol eder.
        if (hittable != null)//eger carp�lan nesne Ihittable arayuzunu uyguluyorsa, ona hasar verir.
        {
           
            hittable.GetHit(1, gameObject);//carp�lan nesneye hasar verir
            health.GetHit(1, gameObject); //mermi kendi sagl�g�ndan da bir hasar al�r
            Destroy(gameObject);//mermi yok edilir
        }
    }

   
}
//ihattable arayuzu, nesnelerin hasar alabilmesi icin kullan�lacak bir arayuzdur.
public interface IHittable
{
    //hasar alma fonksiyonu. her nesne bu arayuzu uygulayarak hasar alabilir
    void GetHit(int damageValue, GameObject sender);
}
