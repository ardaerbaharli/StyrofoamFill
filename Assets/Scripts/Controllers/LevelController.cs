using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public List<LevelConfig> numberOfLevels;
    private int currentLevelIndex = 0;
    private LevelConfig currentLevel;
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    void Start()
    {
        currentLevel = numberOfLevels[0];
        var gameController = GameObject.Find("GameController");
        gameController.GetComponent<GameController>().ConfigureGame(currentLevel);

    }

    public void LoadNextLevel()
    {
        currentLevelIndex++;
        if (currentLevelIndex <= numberOfLevels.Count)
        {
            SceneController.instance.LoadGameScreen();
            currentLevel = numberOfLevels[currentLevelIndex];
            var gameController = GameObject.Find("GameController");
            gameController.GetComponent<GameController>().ConfigureGame(currentLevel);
        }
    }
}
