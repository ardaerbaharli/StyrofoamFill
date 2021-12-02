using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject boxPrefab;
    private Vector3 boxStartPos;
    void Start()
    {
        boxStartPos = new Vector3(-Camera.main.orthographicSize * Camera.main.aspect, -1.73f, 0);
        var box = Instantiate(boxPrefab);
        boxPrefab.transform.position = boxStartPos;
    }

    void Update()
    {

    }
}
