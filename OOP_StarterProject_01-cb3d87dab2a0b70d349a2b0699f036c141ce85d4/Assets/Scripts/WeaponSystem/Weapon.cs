using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PA.WeaponSystem
{
    // Bu sýnýf silahýn davranýþlarýný yönetir.
    public class Weapon : MonoBehaviour
    {
        [SerializeField] List<AttackPatternSO> weapon;// Saldýrý desenlerinin (attack patterns) bir listesidir. bu liste, silahýn kullanabilecegi farklý saldýrý türlerini içerir 
        private int index = 0; // hangi saldýrý deseninin seçili oldugunu belirleyen indeks degeri
        [SerializeField] private AudioClip weaponSwap;// silah degiþtirilirken oynatýlacak ses efekti

        public bool shootingDelayed;// saldýrýlarýn arasýnda beklemek için kullanýlan boolean bayragý, bu bayrak, atýþlarýn gecikmeli olmasýný saglar.

        [SerializeField] private AttackPatternSO attackPattern;// su anda aktif olan saldýrý deseni(attack pattern). Bu, silahýn nasýl ateþ edecegini tanýmlar 
        [SerializeField] private Transform shootingStartPoint;//Mermilerin çýkacagý baslangýc noktasý (Transform). Bu, mermilerin nereden çýkacagýný belirler

        public GameObject projectile;// Ateþlenecek olan mermi prefab'ý

        public AudioSource gunAudio;//silah sesleri çalmak için kullanýlan ses kaynagý 

        // silah deðiþtirme fonksiyonu. Silahýn bir sonraki saldýrý desenine geçmesini saðlar.
        public void SwapWeapon()
        {
            index++;
            index = index >= weapon.Count ? 0 : index;// eger indeks mevcut saldýrý desenlerinin sayýsýný aþarsa, baþa döner(silahlarý döngüsel olarak degiþtirir)
            attackPattern = weapon[index];// þu anda aktif olan saldýrý deseni güncellenir.
           // GetComponent<SpriteRenderer>().sprite = weapon[index].weaponSprite;
            gunAudio.PlayOneShot(weaponSwap); // silah deðiþtirilirken ses çalýnýr.
        }

        //saldýrý (atýs9 gerceklestirmek icin cagrýlýr.
        public void PerformAttack()
        {
            //eger atýs gecikmesi yoksa, saldýrý baþlatýlýr.
            if (shootingDelayed == false)
            {
                shootingDelayed = true;//saldýrý baþlar, bu yüzden gecikme bayragý true yapýlýr
                gunAudio.PlayOneShot(attackPattern.AudioSFX);//saldýrý yaparken saldýrý deseniyel ilþkili ses efekti çalýnýr.
                //GameObject p = Instantiate(projectile, transform.position, Quaternion.identity);
                attackPattern.Perform(shootingStartPoint);//mermi instantiate etmek yerine saldýrý deseni calýstýrýlýr.//attackpattern.perform fonksiyonu, merminin nasýl ateþlecegini yönetir
                StartCoroutine(DelayShooting());//atýsýn tekrar yapýlabilmesi için gecikme baslatýlýr
            } 
        }

        private IEnumerator DelayShooting()//atýsýn tekrar yapýlabilmesi icin  belirli bir sure bekleten fonksiyon
        {
            yield return new WaitForSeconds(attackPattern.AttackDelay);//saldýrý deseninin tanýmladýgý gecikme suresi kadar beklenir
            shootingDelayed = false;//gecikme suresi dolduktan sonra tekrar atýþ yapýlabilir.
        }
    }
}