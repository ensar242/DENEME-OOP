using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

//using static UnityEngine.EventSystems.EventTrigger;

public class BossSpawner : MonoBehaviour
{
    public Boss boss;

    public float timeMin = 0.1f, timeMax = 0.3f;
    public int maxEnemies = 1;

    public BoxCollider2D boxCollider;

    public float startDelay = 1;
    public int waveCount = 1;
    public int currentWave = 0;

    List<GameObject> currentBoss = new List<GameObject>();

    [SerializeField] private TMP_Text scoreText;
    int score;

    public InGameMenu winScreen;
    public Button menuButton;

    ScoreSystem scoreSystem;

    // Start is called before the first frame update
    void Start()
    {
        scoreSystem = FindObjectOfType<ScoreSystem>();// sahnedeki skor sistemini bulur
        score = scoreSystem.currentScore;// skoru skor sisteminden alýr
        StartCoroutine(SpawnWaveWithDelay(startDelay));//ilk dalgayý baþlatýr.
    }

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
            GameObject newBoss = Instantiate(boss.gameObject, spawnPoint, Quaternion.Euler(0, 0, -90));
            currentBoss.Add(newBoss);
            newBoss.GetComponent<Boss>().bossSpawner = this;
            yield return new WaitForSeconds(UnityEngine.Random.Range(timeMin, timeMax));
        }
    }

    public void BossKilled (Boss boss, bool playerKill)
    {
        if (currentBoss.Remove(boss.gameObject))
        {
            if (playerKill)
            {
                score += 2000;
                scoreText.text = score + "";
            }
            
            if(currentBoss.Count == 0)
            {
                if(currentWave == waveCount)
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
