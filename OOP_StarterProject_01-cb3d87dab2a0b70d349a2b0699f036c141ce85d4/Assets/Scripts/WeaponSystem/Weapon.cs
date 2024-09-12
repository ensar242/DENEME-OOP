using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PA.WeaponSystem
{
    // Bu s�n�f silah�n davran��lar�n� y�netir.
    public class Weapon : MonoBehaviour
    {
        [SerializeField] List<AttackPatternSO> weapon;// Sald�r� desenlerinin (attack patterns) bir listesidir. bu liste, silah�n kullanabilecegi farkl� sald�r� t�rlerini i�erir 
        private int index = 0; // hangi sald�r� deseninin se�ili oldugunu belirleyen indeks degeri
        [SerializeField] private AudioClip weaponSwap;// silah degi�tirilirken oynat�lacak ses efekti

        public bool shootingDelayed;// sald�r�lar�n aras�nda beklemek i�in kullan�lan boolean bayrag�, bu bayrak, at��lar�n gecikmeli olmas�n� saglar.

        [SerializeField] private AttackPatternSO attackPattern;// su anda aktif olan sald�r� deseni(attack pattern). Bu, silah�n nas�l ate� edecegini tan�mlar 
        [SerializeField] private Transform shootingStartPoint;//Mermilerin ��kacag� baslang�c noktas� (Transform). Bu, mermilerin nereden ��kacag�n� belirler

        public GameObject projectile;// Ate�lenecek olan mermi prefab'�

        public AudioSource gunAudio;//silah sesleri �almak i�in kullan�lan ses kaynag� 

        // silah de�i�tirme fonksiyonu. Silah�n bir sonraki sald�r� desenine ge�mesini sa�lar.
        public void SwapWeapon()
        {
            index++;
            index = index >= weapon.Count ? 0 : index;// eger indeks mevcut sald�r� desenlerinin say�s�n� a�arsa, ba�a d�ner(silahlar� d�ng�sel olarak degi�tirir)
            attackPattern = weapon[index];// �u anda aktif olan sald�r� deseni g�ncellenir.
           // GetComponent<SpriteRenderer>().sprite = weapon[index].weaponSprite;
            gunAudio.PlayOneShot(weaponSwap); // silah de�i�tirilirken ses �al�n�r.
        }

        //sald�r� (at�s9 gerceklestirmek icin cagr�l�r.
        public void PerformAttack()
        {
            //eger at�s gecikmesi yoksa, sald�r� ba�lat�l�r.
            if (shootingDelayed == false)
            {
                shootingDelayed = true;//sald�r� ba�lar, bu y�zden gecikme bayrag� true yap�l�r
                gunAudio.PlayOneShot(attackPattern.AudioSFX);//sald�r� yaparken sald�r� deseniyel il�kili ses efekti �al�n�r.
                //GameObject p = Instantiate(projectile, transform.position, Quaternion.identity);
                attackPattern.Perform(shootingStartPoint);//mermi instantiate etmek yerine sald�r� deseni cal�st�r�l�r.//attackpattern.perform fonksiyonu, merminin nas�l ate�lecegini y�netir
                StartCoroutine(DelayShooting());//at�s�n tekrar yap�labilmesi i�in gecikme baslat�l�r
            } 
        }

        private IEnumerator DelayShooting()//at�s�n tekrar yap�labilmesi icin  belirli bir sure bekleten fonksiyon
        {
            yield return new WaitForSeconds(attackPattern.AttackDelay);//sald�r� deseninin tan�mlad�g� gecikme suresi kadar beklenir
            shootingDelayed = false;//gecikme suresi dolduktan sonra tekrar at�� yap�labilir.
        }
    }
}