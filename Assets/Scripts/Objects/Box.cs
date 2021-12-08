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
        var startPos = transform.position;
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

        // close the box
        CloseBox();
        yield return new WaitForSeconds(0.5f);

        // move to the side
        t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            transform.position = Vector3.Lerp(transform.position, startPos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
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

    private void CloseBox()
    {
        var topLeft = transform.GetChild(1);
        var topRight = transform.GetChild(2);

        StartCoroutine(Rotate(topLeft, -359));
        StartCoroutine(Rotate(topRight, 359));
    }




    private IEnumerator Rotate(Transform go, float zAngle)
    {

        const float Interval1 = 2.0f;
        Quaternion from = transform.rotation;
        Quaternion to = Quaternion.Euler(0, 0, zAngle);


        for (float t = 0; t < Interval1; t += Time.deltaTime)
        {
            float fraction = t / Interval1;

            // put code here to gently rotate your ship from its tipped position back to upright
            // I created the 'fraction' variable above because it is easy to use it
            // with either Quaternion.Lerp to smoothly turn you from one rotation to another,
            // or to use it with Vector3.Lerp for translation (smooth movement)

            // set the rotation here after calculating it with Quaternion.Lerp and 'fraction'
            go.localRotation = Quaternion.Slerp(from, to, fraction);
            yield return null;   // gotta have this so it waits until the next frame
        }
        Debug.Log("asdfa");
    }
}
