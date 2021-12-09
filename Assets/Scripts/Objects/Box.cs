using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] private float remainingVolume;
    [SerializeField] private GameObject foamParent;
    public readonly float MaxVolume = 400f;
    public float slideSpeed;
    public float index;

    public float RemainingVolume { get => remainingVolume; set => remainingVolume = value; }
    public bool slide;
    public bool isStopped;

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
        const float distanceBetweenBoxes = 4f;
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
        else isStopped = true;

        if (remainingVolume < 0)
            boxStatus = BoxStatus.OverFilled;
        else if (remainingVolume >= 0 && remainingVolume <= MaxVolume * 0.1f)
            boxStatus = BoxStatus.Filled;
        else if (remainingVolume > MaxVolume * 0.1f)
            boxStatus = BoxStatus.Empty;
    }


    public IEnumerator Showcase()
    {
        // remove the foams

        Destroy(foamParent);

        // move to the middle
        var targetPos = new Vector3(0, 0, 0);

        float seconds = 2f;
        float t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            transform.position = Vector3.Lerp(transform.position, targetPos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }

        // show objecsts
        var objects = transform.GetChild(transform.childCount - 1);
        for (int i = 0; i < objects.childCount; i++)
        {
            var obj = objects.GetChild(i);
            var objTargetPos = new Vector3(obj.transform.position.x, obj.transform.position.y + 1f, obj.transform.position.z);
            yield return StartCoroutine(ShowObject(obj.gameObject, objTargetPos));
        }

        yield return new WaitForSeconds(0.2f);
    }

    private IEnumerator ShowObject(GameObject obj, Vector3 target)
    {
        Destroy(obj.GetComponent<Rigidbody>());

        float seconds = 2f;
        float t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            obj.transform.position = Vector3.Lerp(obj.transform.position, target, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);


        var rb = obj.AddComponent<Rigidbody>();
        rb.mass = 10;
        rb.useGravity = true;
    }

    public void OutOfNozzleRange()
    {
        if (boxStatus.Equals(BoxStatus.Filled)) StartCoroutine(gameController.Win(gameObject));
        else StartCoroutine(gameController.Lost(boxStatus));
    }

    public IEnumerator CloseBox()
    {
        var topLeft = transform.GetChild(1);
        var topRight = transform.GetChild(2);

        yield return StartCoroutine(Rotate(topLeft, topRight));

    }

    private IEnumerator RotateLeft(Transform go)
    {
        float targetAnglea = -360;
        float interval = 1f;
        Debug.Log("left " + go.rotation.z);
        var neededAngle = targetAnglea + 110;
        float elapsed = 0f;
        while (go.rotation.z >= targetAnglea + 10)
        {
            elapsed += Time.deltaTime;
            go.rotation = Quaternion.Euler(0, 0, -110 + neededAngle * (elapsed / interval));
            if (elapsed >= interval)
                break;
            yield return null;
        }
        go.rotation = Quaternion.Euler(0, 0, targetAnglea);
        yield return null;
    }

    private IEnumerator RotateRight(Transform go)
    {
        float targetAnglea = 360;
        Debug.Log("right " + go.rotation.z);
        float interval = 1f;
        var neededAngle = targetAnglea - 110;
        float elapsed = 0f;
        while (go.rotation.z <= targetAnglea - 10)
        {
            elapsed += Time.deltaTime;
            go.rotation = Quaternion.Euler(0, 0, 110 + neededAngle * (elapsed / interval));
            if (elapsed >= interval)
                break;
            yield return null;
        }
        go.rotation = Quaternion.Euler(0, 0, targetAnglea);
        yield return null;
    }

    private IEnumerator Rotate(Transform topLeft, Transform topRight)
    {
        StartCoroutine(RotateLeft(topLeft));
        StartCoroutine(RotateRight(topRight));
        yield return null;
    }
}
