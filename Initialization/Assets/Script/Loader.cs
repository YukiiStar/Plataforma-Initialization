using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    public static Loader Instance { get; private set; }


    private void Awake()
    {
        // Implementa o padrão Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }


        // Inicia carregando a Splash
        LoadScene("Splash");
    }


    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }


    public void LoadSceneDelayed(string sceneName, float delay)
    {
        StartCoroutine(LoadSceneAfterDelay(sceneName, delay));
    }


    private System.Collections.IEnumerator LoadSceneAfterDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        LoadScene(sceneName);
    }
}