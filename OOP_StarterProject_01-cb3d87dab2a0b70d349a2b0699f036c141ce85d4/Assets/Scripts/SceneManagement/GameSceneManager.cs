using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public int mainMenuSceneIndex, level1SceneIndex;//ana men� ve level 1 sahnelerinin indeksleri

    //Ana men� sahnesini y�kleyen fonksiyon
    public void LoadMenu()
    {
        LoadScene(mainMenuSceneIndex);//mainMenuSceneIndex'teki sahneyi y�kler
    }

    //belirtilen indeks numaras�na g�re sahne y�kleyen �zel bir fonksiyon
    private void LoadScene(int index)
    {
        SceneManager.LoadScene(index);// unity'nin sceneManager'�n� kullanarak sahne degistirir
    }
    //Level 1 sahnesini y�kleyen fonksiyon
    public void LoadLevel()
    {
        LoadScene(level1SceneIndex);//level1SceneIndex'teki sahneyi y�kler
    }
    //mevcut sahneyi yeniden ba�latan fonksiyon
    public void RestartLevel()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex);//�u anda aktif olan sahnenin indeksini al�p yeniden y�kler
    }
    //bir sonraki sahneyi y�kleyen fonksiyon
    public void LoadNextLevel()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex+1);//mevcut sahnenin indeksine 1 ekleyerek bir sonraki sahneyi y�kler
    }
}
