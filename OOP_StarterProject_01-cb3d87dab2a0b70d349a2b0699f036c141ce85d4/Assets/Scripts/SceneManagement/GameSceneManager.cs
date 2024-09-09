using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public int mainMenuSceneIndex, level1SceneIndex;//ana menü ve level 1 sahnelerinin indeksleri

    //Ana menü sahnesini yükleyen fonksiyon
    public void LoadMenu()
    {
        LoadScene(mainMenuSceneIndex);//mainMenuSceneIndex'teki sahneyi yükler
    }

    //belirtilen indeks numarasýna göre sahne yükleyen özel bir fonksiyon
    private void LoadScene(int index)
    {
        SceneManager.LoadScene(index);// unity'nin sceneManager'ýný kullanarak sahne degistirir
    }
    //Level 1 sahnesini yükleyen fonksiyon
    public void LoadLevel()
    {
        LoadScene(level1SceneIndex);//level1SceneIndex'teki sahneyi yükler
    }
    //mevcut sahneyi yeniden baþlatan fonksiyon
    public void RestartLevel()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex);//þu anda aktif olan sahnenin indeksini alýp yeniden yükler
    }
    //bir sonraki sahneyi yükleyen fonksiyon
    public void LoadNextLevel()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex+1);//mevcut sahnenin indeksine 1 ekleyerek bir sonraki sahneyi yükler
    }
}
