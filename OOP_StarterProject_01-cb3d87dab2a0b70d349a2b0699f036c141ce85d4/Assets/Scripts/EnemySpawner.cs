using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;// TextMesh Pro'yu kullanmak i�in gerekli
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour   
{
    
    public Enemy enemy;// spawn edilecek d��man t�r�n�n belirlenmesi
    
    public float timeMin = 0.1f, timeMax = 0.3f;//d��manlar�n ��k�� s�releri aras�ndaki rastgele aral�k 
    public int maxEnemies = 3;// her dalgada ka� d��man spawn edilecegini belirtiyor.

    public BoxCollider2D boxCollider;// d��manlar�n nerede spawn edilece�ini belirten alan 

    public float startDelay = 1;//ilk dalgan�n ba�lamas� i�in ge�en gecikme s�resi
    public int waveCount = 5;// ka� dalga olacag�n� belirler
    public int currentWave = 0;//�u anda ka��nc� dalgada oldugunu tutar(buradaki degeri kullanarak bossun en son gelmesi gerektigi zaman� bulabiliriz)

    List<GameObject> currentEnemies = new List<GameObject>();// �u anda var olan d��manlar�n listesi 

    [SerializeField]
    private TMP_Text scoreText;//UI'da skor bilgisini g�sterecek TextMesh Pro bile�eni
    int score;

    public InGameMenu winScreen;// oyuncu kazand�g�nda g�sterilecek kazanma ekran� 
    public Button menuButton;// oyun kazand�g�nda devre d��� b�rak�lacak men� butonu

    ScoreSystem scoreSystem;// skor sistemini y�neten bile�en

    // Start is called before the first frame update
    void Start()
    {
        scoreSystem = FindObjectOfType<ScoreSystem>();// sahnedeki skor sistemini bulur
        score = scoreSystem.currentScore;// skoru skor sisteminden al�r
        StartCoroutine(SpawnWaveWithDelay(startDelay));//ilk dalgay� ba�lat�r.
    }

    // belirli bir gecikmeden sonra dalgay� ba�latmak i�in bir coroutine
    private IEnumerator SpawnWaveWithDelay(float startDelay)
    {
        currentWave++;
        yield return new WaitForSeconds(startDelay);// belirtilen gecikme kadar bekler
        float minX = boxCollider.bounds.min.x;// spawn alan�n�n sol s�n�r�
        float maxX = boxCollider.bounds.max.x;//spawn alan�n�n sa� s�n�r�
        
        // maksimum d��man say�s� kadar d��man spawn eder 
        for (int i = 0; i < maxEnemies; i++)
        {
            Vector3 spawnPoint = new Vector3(UnityEngine.Random.Range(minX, maxX), transform.position.y, 0);
            GameObject newEnemy = Instantiate(enemy.gameObject, spawnPoint, Quaternion.Euler(0, 0, -90));
            currentEnemies.Add(newEnemy);
            newEnemy.GetComponent<Enemy>().enemySpawner = this;
            yield return new WaitForSeconds(UnityEngine.Random.Range(timeMin, timeMax));
        }
        
    }

    public void EnemyKilled(Enemy enemy, bool playerKill)
    {
        if (currentEnemies.Remove(enemy.gameObject))
        {
            if (playerKill)
            {
                score += 100;
                scoreText.text = score + "";
            }

            if (currentEnemies.Count == 0)
            {
                if (currentWave == waveCount)
                {
                    Debug.Log("You win");
                    scoreSystem.SetScore(score);
                    winScreen.Toggle();
                    menuButton.interactable = false;
                    return;
                }
                StartCoroutine(SpawnWaveWithDelay(0.5f));
            }
        }
        
    }
}
