using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject boxPrefab;
    [SerializeField] private GameObject pLineTile;
    [SerializeField] private Transform pLine;
    [SerializeField] private GameObject progressBar;
    [SerializeField] private GameObject playerObj;
    [SerializeField] private GameObject levelOverMenu;
    [SerializeField] private Transform canvas;
    [SerializeField] private List<GameObject> objects;
    [SerializeField] [Range(0 , 10f)] private float gameSpeed=1f;
    private Player player;
    private List<GameObject> boxes;
    private List<GameObject> pLineTiles;
    private LevelConfig levelConfig;
    private LevelController levelController;
    private int playableNumberOfBoxes;
    private float tileWidth;
    private float boxWidth;

    void Awake()
    {
        boxWidth = boxPrefab.transform.GetChild(0).GetComponent<MeshRenderer>().bounds.size.x;
        tileWidth = pLineTile.GetComponent<MeshRenderer>().bounds.size.x;

        progressBar.GetComponent<Slider>().maxValue = boxPrefab.GetComponent<Box>().MaxVolume;
        player = playerObj.GetComponent<Player>();

        boxes = new List<GameObject>();
        pLineTiles = new List<GameObject>();
    }
    private void Update()
    {
        Time.timeScale = gameSpeed;
    }

    public void ConfigureGame(LevelController levelController)
    {
        this.levelController = levelController;
        levelConfig = levelController.currentLevelConfig;
        CreateBox(levelConfig);
        CreateProductionLine();
    }

    private void CreateBox(LevelConfig levelConfig)
    {
        const float distanceBetweenBoxes = 4f;
        for (int i = 0; i < levelConfig.box.Count; i++)
        {
            var box = levelConfig.box[i];
            float boxStartPosX = -Camera.main.orthographicSize * Camera.main.aspect - boxWidth - (distanceBetweenBoxes * i);
            var boxStartPos = new Vector3(boxStartPosX, -1.73f, 0);
            var boxObj = Instantiate(boxPrefab);
            boxObj.name = $"Box{i}";
            boxObj.GetComponent<Box>().slideSpeed = levelConfig.gameSpeed;
            boxObj.GetComponent<Box>().index = (levelConfig.box.Count-1) - i;
            Transform boxTransform = boxObj.transform;
            boxTransform.position = boxStartPos;

            for (int j = 0; j < box.item.Count; j++)
            {
                var item = box.item[j];
                var objStartPos = new Vector3(boxStartPos.x, 0 + j, boxStartPos.z);
                int index = item.index;
                var obj = Instantiate(objects[index], boxTransform.GetChild(boxTransform.childCount - 1));
                obj.transform.position = objStartPos;
                boxObj.GetComponent<Box>().RemainingVolume -= obj.GetComponent<BoxObject>().Volume;
            }
            boxes.Add(boxObj);
        }
        playableNumberOfBoxes = boxes.Count;
    }


    private void CreateProductionLine()
    {
        int tileCount = 40;
        for (int i = 0; i < tileCount; i++)
        {
            var tile = Instantiate(pLineTile, pLine);
            tile.GetComponent<ProductionLineTile>().slideSpeed = levelConfig.gameSpeed;

            tile.name = "tile";
            tile.transform.position = GetTilePos(i);
            pLineTiles.Add(tile);
        }
    }

    private Vector3 GetTilePos(int i)
    {
        float tilePosX = Camera.main.orthographicSize * Camera.main.aspect - (i * tileWidth) - (i * 0.1f);
        var tilePos = new Vector3(tilePosX, -1.869f, 0.311f);
        return tilePos;
    }


    public void Lost(BoxStatus boxStatus)
    {
        playableNumberOfBoxes--;
        if (boxStatus.Equals(BoxStatus.Empty))
        {
            Debug.Log("Objects are broken");
            player.LoseMoney();
        }
        else if (boxStatus.Equals(BoxStatus.OverFilled))
        {
            Debug.Log("Box cant be closed");
            player.LoseMoney();
        }
        if (playableNumberOfBoxes == 0)
            LevelOver();
    }


    public void Win()
    {
        playableNumberOfBoxes--;
        Debug.Log("Box is filled successfully");
        player.WinMoney();
        if (playableNumberOfBoxes == 0)
            LevelOver();
    }

    public void LevelOver()
    {
        // ShowMenu();
    }

    private void ShowMenu()
    {
        DestroyOrStopGameObjects();
        var menu = Instantiate(levelOverMenu, canvas);
        menu.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { levelController.LoadNextLevel(); });
        menu.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(delegate { levelController.RestartLevel(); });
    }

    private void DestroyOrStopGameObjects()
    {
        foreach (var box in boxes)
        {
            box.GetComponent<Box>().slide = false;
        }

        foreach (var tile in pLineTiles)
        {
            tile.GetComponent<ProductionLineTile>().slide = false;
        }

        Destroy(GameObject.Find("Nozzle"));
        Destroy(progressBar);
    }
}
