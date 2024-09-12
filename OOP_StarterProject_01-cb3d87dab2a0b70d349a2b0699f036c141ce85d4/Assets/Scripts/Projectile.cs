using PA.HealthSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//merminin davranýþlarýný yönetir.
public class Projectile : MonoBehaviour
{
    public float speed = 10;//merminin hareket hýzý
    public Rigidbody2D rb2d;// merminin fiziksel hareketini yönetmek için kullanýlan rigidbody2D bileþeni
    public float deathDelay = 5;//merminin sahnede ne kadar süre aktif kalacagýný belirleyen eleman 

    [SerializeField] private int initialHealth = 5;//merminin baþlangýç saðlýgý 
    [SerializeField] private Health health;//merminin saglýgýný yönetecek olan health bileþeni

    //public bool disabled = false;

    // Start is called before the first frame update

    //start metodu, mermi sahnede aktif oldugunda bir kez cagrýlýr
    void Start()
    {
        health = GetComponent<Health>();//merminin health bileþeni alýnýr ev baþlangýç saglýgý atanýr
        health.InitializeHealth(initialHealth);


        rb2d.velocity = transform.up * speed;//merminin hareket yönü ve hýzý belirlenir.Transform'un "up" yönüne göre hýz verilir
        StartCoroutine(DeathAfterDelay(deathDelay));//belirtilen bir süre sonra merminin yok edilmesi için bir coroutine baþlatýlýr.
    }

    private IEnumerator DeathAfterDelay(float deathDelay)//bu corutuine, belirli bir süre bekledikten sonra mermiyi yok eder
    {
        yield return new WaitForSeconds(deathDelay);//belirtilen süre kadar bekler
        health.GetHit(1,gameObject);//merminin bir hasar almasýný simule eder
        Destroy(gameObject);//mermiyi yok eder
    }
    //mermi baþka bir collider ile carpýstýgýnda bu fonksiyon tetiklenir
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IHittable hittable = collision.GetComponent<IHittable>();//eger carpýlan nesne Ihittable arayüzünü uygulayýp uygulamadýgýný kontrol eder.
        if (hittable != null)//eger carpýlan nesne Ihittable arayuzunu uyguluyorsa, ona hasar verir.
        {
           
            hittable.GetHit(1, gameObject);//carpýlan nesneye hasar verir
            health.GetHit(1, gameObject); //mermi kendi saglýgýndan da bir hasar alýr
            Destroy(gameObject);//mermi yok edilir
        }
    }

   
}
//ihattable arayuzu, nesnelerin hasar alabilmesi icin kullanýlacak bir arayuzdur.
public interface IHittable
{
    //hasar alma fonksiyonu. her nesne bu arayuzu uygulayarak hasar alabilir
    void GetHit(int damageValue, GameObject sender);
}
