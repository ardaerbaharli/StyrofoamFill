using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    private float remainingVolume = 400f;

    public float RemainingVolume { get => remainingVolume; set => remainingVolume = value; }

    private void Update()
    {

        transform.position += Vector3.right * 0.5f * Time.deltaTime;
    }
}
