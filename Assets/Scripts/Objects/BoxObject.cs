using UnityEngine;

public class BoxObject : MonoBehaviour
{
    [SerializeField] private float _volume;
    [SerializeField] private string _name;

    public float Volume { get => _volume; set => _volume = value; }
    public string Name { get => _name; set => _name = value; }
}
