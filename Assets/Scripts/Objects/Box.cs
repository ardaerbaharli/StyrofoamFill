using System;
using System.Collections;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] private float remainingVolume;
    public readonly float MaxVolume = 500f;
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
        // move to the middle
        //var startPos = transform.position;
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

        yield return new WaitForSeconds(1f);
        //// close the box
        //CloseBox();
        //yield return new WaitForSeconds(0.5f);

        //// move to the side
        //float t = 0f;
        //while (t <= 1.0)
        //{
        //    t += Time.deltaTime / seconds;
        //    transform.position = Vector3.Lerp(transform.position, startPos, Mathf.SmoothStep(0f, 1f, t));
        //    yield return null;
        //}
        //yield return new WaitForSeconds(0.5f);

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

        yield return new WaitForSeconds(1f);


        var rb = obj.AddComponent<Rigidbody>();
        rb.mass = 10;
        rb.useGravity = true;
    }

    public void OutOfNozzleRange()
    {
        if (boxStatus.Equals(BoxStatus.Filled))
        {
            //CloseBox();
            StartCoroutine(gameController.Win(gameObject));
        }
        else StartCoroutine(gameController.Lost(boxStatus));

    }

    public IEnumerator CloseBox()
    {
        var topLeft = transform.GetChild(1);
        var topRight = transform.GetChild(2);

        yield return StartCoroutine(Rotate(topLeft, -359));
        yield return StartCoroutine(Rotate(topRight, 359));
    }


    private IEnumerator Rotate(Transform go, float zAngle)
    {

        for (int i = 1; i < 5; i++)
        {
            float interval = 0.25f;
            Quaternion startAngle = go.rotation;
            Quaternion targetAngle = Quaternion.Euler(0, 0, zAngle / 5 - i);
            float elapsed = 0f;
            while (elapsed < interval)
            {
                go.rotation = Quaternion.Lerp(startAngle, targetAngle, elapsed / 2);
                elapsed += Time.deltaTime;
                yield return null;
            }
            go.rotation = targetAngle;
            yield return null;
        }

    }
}
