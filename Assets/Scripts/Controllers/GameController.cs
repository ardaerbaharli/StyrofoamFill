using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject boxPrefab;
    [SerializeField] private GameObject progressBar;
    [SerializeField] private List<GameObject> objects;
    public static GameController instance;
    void Start()
    {
        progressBar.GetComponent<Slider>().maxValue = boxPrefab.GetComponent<Box>().MaxVolume;

        int[][] itemIndexes = new int[][] {
            new int[] { 0, 3 },
            new int[] { 2, 1 },
           };

        int[] itemCounts = new int[itemIndexes.Length];

        for (int i = 0; i < itemIndexes.Length; i++)
        {
            itemCounts[i] = itemIndexes[i].Length;
        }

        int amount = itemCounts.Length;

        CreateBox(amount, itemCounts, itemIndexes);
    }

    private void CreateBox(int boxCount, int[] itemCounts, int[][] itemIndexes)
    {
        float boxWidth = boxPrefab.transform.GetChild(0).GetComponent<MeshRenderer>().bounds.size.x;

        for (int i = 0; i < boxCount; i++)
        {
            float boxStartPosX = -Camera.main.orthographicSize * Camera.main.aspect - (2.5f * i * boxWidth);
            var boxStartPos = new Vector3(boxStartPosX, -1.73f, 0);
            var box = Instantiate(boxPrefab);
            Transform boxTransform = box.transform;
            boxTransform.position = boxStartPos;

            for (int j = 0; j < itemCounts[i]; j++)
            {
                var objStartPos = new Vector3(boxStartPos.x, 0 + j, boxStartPos.z);
                int index = itemIndexes[i][j];
                var obj = Instantiate(objects[index], boxTransform.GetChild(boxTransform.childCount - 1));
                obj.transform.position = objStartPos;
                box.GetComponent<Box>().RemainingVolume -= obj.GetComponent<BoxObject>().Volume;
            }
        }
    }

    public void Lost(BoxSituation boxSituation)
    {
        if (boxSituation.Equals(BoxSituation.Empty))
        {
            Debug.Log("Objects are broken");
        }
        else if (boxSituation.Equals(BoxSituation.OverFilled))
        {
            Debug.Log("Box cant be closed");
        }
    }
}
