using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
public class LoaderSplash : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      
        StartCoroutine(LoadMainMenuAfterDelay());
    }
      
    IEnumerator LoadMainMenuAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        Loader.Instance.LoadScene("MainMenu");
    }


    // Update is called once per frame
    void Update()
    {
      
    }
}