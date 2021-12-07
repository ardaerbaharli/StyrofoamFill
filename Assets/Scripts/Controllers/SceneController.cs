using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    void Start()
    {
        instance = this;
    }
    private void Update()
    {
        if (instance == null)
            instance = this;
    }
    public void LoadGameScreen()
    {
        StartCoroutine(LoadAsyncScene("Level1"));
    }


    public IEnumerator LoadAsyncScene(string sceneName)
    {
        System.GC.Collect();
        Resources.UnloadUnusedAssets();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
