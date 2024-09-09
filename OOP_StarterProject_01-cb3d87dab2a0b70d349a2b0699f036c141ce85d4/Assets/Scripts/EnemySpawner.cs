using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;// TextMesh Pro'yu kullanmak için gerekli
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour   
{
    
    public Enemy enemy;// spawn edilecek düþman türünün belirlenmesi
    
    public float timeMin = 0.1f, timeMax = 0.3f;//düþmanlarýn çýkýþ süreleri arasýndaki rastgele aralýk 
    public int maxEnemies = 3;// her dalgada kaç düþman spawn edilecegini belirtiyor.

    public BoxCollider2D boxCollider;// düþmanlarýn nerede spawn edileceðini belirten alan 

    public float startDelay = 1;//ilk dalganýn baþlamasý için geçen gecikme süresi
    public int waveCount = 5;// kaç dalga olacagýný belirler
    public int currentWave = 0;//þu anda kaçýncý dalgada oldugunu tutar(buradaki degeri kullanarak bossun en son gelmesi gerektigi zamaný bulabiliriz)

    List<GameObject> currentEnemies = new List<GameObject>();// þu anda var olan düþmanlarýn listesi 

    [SerializeField]
    private TMP_Text scoreText;//UI'da skor bilgisini gösterecek TextMesh Pro bileþeni
    int score;

    public InGameMenu winScreen;// oyuncu kazandýgýnda gösterilecek kazanma ekraný 
    public Button menuButton;// oyun kazandýgýnda devre dýþý býrakýlacak menü butonu

    ScoreSystem scoreSystem;// skor sistemini yöneten bileþen

    // Start is called before the first frame update
    void Start()
    {
        scoreSystem = FindObjectOfType<ScoreSystem>();// sahnedeki skor sistemini bulur
        score = scoreSystem.currentScore;// skoru skor sisteminden alýr
        StartCoroutine(SpawnWaveWithDelay(startDelay));//ilk dalgayý baþlatýr.
    }

    // belirli bir gecikmeden sonra dalgayý baþlatmak için bir coroutine
    private IEnumerator SpawnWaveWithDelay(float startDelay)
    {
        currentWave++;
        yield return new WaitForSeconds(startDelay);// belirtilen gecikme kadar bekler
        float minX = boxCollider.bounds.min.x;// spawn alanýnýn sol sýnýrý
        float maxX = boxCollider.bounds.max.x;//spawn alanýnýn sað sýnýrý
        
        // maksimum düþman sayýsý kadar düþman spawn eder 
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
