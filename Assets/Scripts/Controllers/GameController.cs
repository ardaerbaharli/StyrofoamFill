using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading.Tasks;
using TMPro;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject boxPrefab;
    [SerializeField] private GameObject pLineTile;
    [SerializeField] private Transform pLine;

    [SerializeField] private GameObject progressBar;
    [SerializeField] private GameObject labelPrinter;
    [SerializeField] private GameObject playerObj;
    [SerializeField] private GameObject levelOverMenu;
    [SerializeField] private GameObject reaction;
    [SerializeField] private Transform canvas;
    [SerializeField] private List<GameObject> objects;
    [SerializeField] [Range(0, 10f)] private float gameSpeed;

    private Player player;
    private List<GameObject> boxes;
    private int numberOfBoxes;
    private List<GameObject> showCaseBoxes;
    private List<GameObject> pLineTiles;
    private LevelConfig levelConfig;
    private LevelController levelController;



    private float tileWidth;
    private float boxWidth;

    void Awake()
    {
        gameSpeed = 1f;
        SpeedUp();

        boxWidth = boxPrefab.transform.GetChild(0).GetComponent<MeshRenderer>().bounds.size.x;
        tileWidth = pLineTile.GetComponent<MeshRenderer>().bounds.size.x;

        progressBar.GetComponent<Slider>().maxValue = boxPrefab.GetComponent<Box>().MaxVolume;
        player = playerObj.GetComponent<Player>();

        boxes = new List<GameObject>();
        pLineTiles = new List<GameObject>();
        showCaseBoxes = new List<GameObject>();
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
        numberOfBoxes = levelConfig.box.Count;
        CreateProductionLine();
    }

    private void CreateBox(LevelConfig levelConfig)
    {
        const float distanceBetweenBoxes = 4f;
        for (int i = 0; i < levelConfig.box.Count; i++)
        {
            var box = levelConfig.box[i];
            float boxStartPosX = -Camera.main.orthographicSize * Camera.main.aspect - boxWidth - (distanceBetweenBoxes * i);
            var boxStartPos = new Vector3(boxStartPosX, -1.817f, 0);
            var boxObj = Instantiate(boxPrefab);
            boxObj.name = $"Box{i}";
            boxObj.GetComponent<Box>().slideSpeed = levelConfig.gameSpeed;
            boxObj.GetComponent<Box>().index = (levelConfig.box.Count - 1) - i;
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
            pLineTiles.Add(tile);
        }
    }

    private Vector3 GetTilePos(int i)
    {
        float tilePosX = Camera.main.orthographicSize * Camera.main.aspect - (i * tileWidth) - (i * 0.1f);
        var tilePos = new Vector3(tilePosX, -1.869f, 0.311f);
        return tilePos;
    }


    public IEnumerator Lost(BoxStatus boxStatus)
    {
        CreateReaction("Oh no!", Color.red);

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

        numberOfBoxes--;
        if (numberOfBoxes == 0)
        {
            SpeedUp();
            while (AreBoxesStopped() == false)
                yield return null;
            StartCoroutine(LevelOver());
        }
    }


    public IEnumerator Win(GameObject gameObject)
    {
        CreateReaction("Keep it up!", Color.green);

        showCaseBoxes.Add(gameObject);

        Debug.Log("Box is filled successfully");
        player.WinMoney();

        numberOfBoxes--;
        if (numberOfBoxes == 0)
        {
            SpeedUp();
            while (AreBoxesStopped() == false)
                yield return null;
            StartCoroutine(LevelOver());
        }
    }

    private bool AreBoxesStopped()
    {
        bool result = true;
        foreach (var item in boxes)
        {
            if (!item.GetComponent<Box>().isStopped)
                result = false;
        }
        return result;
    }

    public IEnumerator LevelOver()
    {
        DestroyOrStopGameObjects();

        foreach (var box in showCaseBoxes)
        {
            var startPos = box.transform.position;

            Slower();

            yield return StartCoroutine(box.GetComponent<Box>().Showcase());
            yield return StartCoroutine(PrintNameOnTheBox(box));

            yield return new WaitForSeconds(0.5f);
            // close the box
            StartCoroutine(box.GetComponent<Box>().CloseBox());

            yield return new WaitForSeconds(0.5f);

            // move to the side
            float seconds = 2f;
            float t = 0f;
            while (t <= 1.0)
            {
                t += Time.deltaTime / seconds;
                box.transform.position = Vector3.Lerp(box.transform.position, startPos, Mathf.SmoothStep(0f, 1f, t));
                yield return null;
            }
            yield return new WaitForSeconds(0.5f);
        }

        ShowMenu();
    }

    private IEnumerator PrintNameOnTheBox(GameObject box)
    {
        var printer = Instantiate(labelPrinter);
        var screenWidth = -Camera.main.orthographicSize * Camera.main.aspect;
        var printerWidth = labelPrinter.GetComponent<MeshRenderer>().bounds.size.x;
        printer.transform.position = new Vector3(screenWidth - printerWidth, 0, 0);

        var startPos = printer.transform.position;
        var targetPos = box.transform.position + new Vector3(-0.4f, 0.35f, -1.25f);

        // slide to the box

        float seconds = 2f;
        float t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            printer.transform.position = Vector3.Lerp(printer.transform.position, targetPos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }

        // collect objects names
        string label = "";
        var objects = box.transform.GetChild(box.transform.childCount - 1);
        for (int i = 0; i < objects.childCount; i++)
        {
            label += objects.GetChild(i).GetComponent<BoxObject>().Name + "\n";
        }
        box.GetComponentInChildren<TextMeshPro>().text = label;

        // move slightly
        seconds = 1f;
        targetPos += new Vector3(1f, 0, 0);
        t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            printer.transform.position = Vector3.Lerp(printer.transform.position, targetPos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }

        yield return new WaitForSeconds(0.3f);

        // go back
        seconds = 2f;
        t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            printer.transform.position = Vector3.Lerp(printer.transform.position, startPos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        Destroy(printer);

    }

    private void ShowMenu()
    {
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

    private void CreateReaction(string reactionText, Color c)
    {
        var r = Instantiate(reaction, canvas);
        r.name = "Reaction";
        r.GetComponent<RectTransform>().localPosition = GetRandomReactionPos();

        r.GetComponentInChildren<Text>().text = reactionText;
        r.GetComponentInChildren<Text>().color = c;       
    }

    private Vector3 GetRandomReactionPos()
    {
        var xOffset = Screen.width * 0.8f;
        var yOffset = Screen.height * 0.8f;
        float x = Random.Range(-xOffset / 2, xOffset / 2);
        float y = Random.Range(-yOffset / 2, yOffset / 2);
        return new Vector3(x, y, 0);
    }



    public void SpeedUp()
    {
        gameSpeed = 3f;
    }
    public void SlowDown()
    {
        gameSpeed = 1.5f;
    }
    public void Slower()
    {
        gameSpeed = 1f;
    }
}
