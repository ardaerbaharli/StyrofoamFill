using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void LoadGameScreen()
    {
        StartCoroutine(LoadAsyncScene("Game"));
    }

    public void LoadMainScreen()
    {
        StartCoroutine(LoadAsyncScene("Main"));
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
