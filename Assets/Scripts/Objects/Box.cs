using System;
using System.Collections;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] private float remainingVolume;
    public readonly float MaxVolume = 400f;
    public float slideSpeed;
    public float index;

    public float RemainingVolume { get => remainingVolume; set => remainingVolume = value; }
    public bool slide;

    private float screenLeftBorderX, screenRightBorderX;
    private float boxWidth;
    private float targetPosX;
    private BoxStatus boxStatus;
    private GameController gameController;

    private void Awake()
    {
        RemainingVolume = MaxVolume;
    }
    private void Start()
    {
        screenRightBorderX = Camera.main.orthographicSize * Camera.main.aspect;
        screenLeftBorderX = -Camera.main.orthographicSize * Camera.main.aspect;
        boxWidth = transform.GetChild(0).GetComponent<MeshRenderer>().bounds.size.x;
        const float distanceBetweenBoxes = 2.5f;
        var startPosX = transform.position.x;
        var space = screenLeftBorderX - startPosX;

        targetPosX = transform.position.x + (screenRightBorderX - screenLeftBorderX) + space + boxWidth + (distanceBetweenBoxes * index);

        slide = true;
        gameController = FindObjectOfType<GameController>();
    }

    private void Update()
    {
        if (slide && transform.position.x < targetPosX)
            transform.position += slideSpeed * Time.deltaTime * Vector3.right;

        if (remainingVolume < 0)
            boxStatus = BoxStatus.OverFilled;
        else if (remainingVolume >= 0 && remainingVolume <= MaxVolume * 0.1f)
            boxStatus = BoxStatus.Filled;
        else if (remainingVolume > MaxVolume * 0.1f)
            boxStatus = BoxStatus.Empty;
    }


    public void SlideToMiddle()
    {

    }


    public void OutOfNozzleRange()
    {
        if (boxStatus.Equals(BoxStatus.Filled))
        {
            CloseBox();
            gameController.Win();
        }
        else gameController.Lost(boxStatus);

    }

    private void CloseBox()
    {
        var topLeft = transform.GetChild(1);
        var topRight = transform.GetChild(2);
        StartCoroutine(Rotate(topLeft));
        StartCoroutine(Rotate(topRight));
    }

    private IEnumerator Rotate(Transform go)
    {
        var v = new Vector3(180, 0, 0);
        var q = Quaternion.Euler(v);

        float seconds = 0.6f;
        float t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            go.rotation = Quaternion.Lerp(go.rotation, q, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }

    }
}
