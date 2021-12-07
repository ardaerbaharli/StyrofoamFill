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

    private Player player;
    private List<GameObject> boxes;
    private LevelConfig levelConfig;
    private LevelController levelController;
    private float tileWidth;
    private float boxWidth;

    void Awake()
    {

        boxWidth = boxPrefab.transform.GetChild(0).GetComponent<MeshRenderer>().bounds.size.x;
        tileWidth = pLineTile.GetComponent<MeshRenderer>().bounds.size.x;

        progressBar.GetComponent<Slider>().maxValue = boxPrefab.GetComponent<Box>().MaxVolume;
        player = playerObj.GetComponent<Player>();
        boxes = new List<GameObject>();
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
        for (int i = 0; i < levelConfig.box.Count; i++)
        {
            var box = levelConfig.box[i];
            float boxStartPosX = -Camera.main.orthographicSize * Camera.main.aspect - (2.5f * i * boxWidth);
            var boxStartPos = new Vector3(boxStartPosX, -1.73f, 0);
            var boxObj = Instantiate(boxPrefab);
            boxObj.GetComponent<Box>().slideSpeed = levelConfig.gameSpeed;
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
    }


    public void Win()
    {
        Debug.Log("Box is filled successfully");
        player.WinMoney();
    }

    public void LevelOver()
    {
        ShowMenu();
    }

    private void ShowMenu()
    {
        var menu = Instantiate(levelOverMenu, canvas);
        DestroyGameObjects();
        menu.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { levelController.LoadNextLevel(); });
        menu.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(delegate { levelController.RestartLevel(); });
    }

    private void DestroyGameObjects()
    {
        foreach (var box in boxes)
        {
            Destroy(box);
        }
        Destroy(GameObject.Find("Nozzle"));
        Destroy(progressBar);
    }
}
