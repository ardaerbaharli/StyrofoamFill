using System.Collections.Generic;
using UnityEngine;

public class BoxObject : MonoBehaviour
{
    [SerializeField] private float _volume;
    [SerializeField] private string _name;

    public float Volume { get => _volume; set => _volume = value; }
    public string Name { get => _name; set => _name = value; }

    private List<Color> colors = new List<Color>()
    { Color.red, Color.green, Color.yellow, Color.blue, Color.magenta, Color.cyan };

    private void Start()
    {
        var r = Random.Range(0, colors.Count);
        GetComponent<MeshRenderer>().material.color = colors[r];
    }
}
