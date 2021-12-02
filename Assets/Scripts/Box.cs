using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    private float volume = 100f;
    private float remainingVolume = 100f;

    public float Volume { get => volume; set => volume = value; }
    public float RemainingVolume { get => remainingVolume; set => remainingVolume = value; }
}
