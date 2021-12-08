using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public List<LevelConfig> levelConfigs;
    public int currentLevelIndex;
    public LevelConfig currentLevelConfig;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        currentLevelIndex = 0;
        LoadLevel(currentLevelIndex);
    }
    public void RestartLevel()
    {
        LoadGameScreen(currentLevelIndex);
    }
    public void LoadNextLevel()
    {
        currentLevelIndex++;
        if (currentLevelIndex < levelConfigs.Count)
        {
            LoadGameScreen(currentLevelIndex);
        }
    }

    private void LoadLevel(int level)
    {
        currentLevelConfig = levelConfigs[level];
        var gameController = GameObject.Find("GameController");
        gameController.GetComponent<GameController>().ConfigureGame(this);
    }

    public void LoadGameScreen(int level)
    {
        StartCoroutine(LoadAsyncScene("Game", level));
    }

    public IEnumerator LoadAsyncScene(string sceneName, int level)
    {
        System.GC.Collect();
        Resources.UnloadUnusedAssets();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        LoadLevel(level);
    }
}
