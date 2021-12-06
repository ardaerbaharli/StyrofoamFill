using UnityEngine;

public class Box : MonoBehaviour
{
    public readonly float MaxVolume = 400f;
    [SerializeField] private float remainingVolume;

    public float RemainingVolume { get => remainingVolume; set => remainingVolume = value; }

    private void Awake()
    {
        RemainingVolume = MaxVolume;
    }

    private void Update()
    {
        transform.position += Vector3.right * 0.5f * Time.deltaTime;
    }
}
