using System;
using System.Collections;
using UnityEngine;

public class Box : MonoBehaviour
{
    public readonly float MaxVolume = 400f;
    [SerializeField] private float remainingVolume;
    private BoxSituation boxSituation;
    private GameController gameController;
    public float RemainingVolume { get => remainingVolume; set => remainingVolume = value; }

    private void Awake()
    {
        RemainingVolume = MaxVolume;
    }
    private void Start()
    {
        gameController = FindObjectOfType<GameController>();

    }

    private void Update()
    {
        transform.position += Vector3.right * 0.5f * Time.deltaTime;

        if (remainingVolume < 0)
            boxSituation = BoxSituation.OverFilled;
        else if (remainingVolume >= 0 && remainingVolume <= MaxVolume * 0.1f)
            boxSituation = BoxSituation.Filled;
        else if (remainingVolume > MaxVolume * 0.1f)
            boxSituation = BoxSituation.Empty;
    }

    public void OutOfNozzleRange()
    {
        if (boxSituation.Equals(BoxSituation.Filled)) CloseBox();
        else gameController.Lost(boxSituation);
    }

    private void CloseBox()
    {
        var topLeft = transform.GetChild(1);
        var topRight = transform.GetChild(2);
        StartCoroutine(Rotate(topLeft));
        //topRight.rotation = Quaternion.Euler(new Vector3(180, 0, 0));
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
