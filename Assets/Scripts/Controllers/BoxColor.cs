using UnityEngine;

public class BoxColor : MonoBehaviour
{
    private void Start()
    {
        GetComponent<MeshRenderer>().material.color =  new Color(0.72f, 0.63f, 0.26f);
    }
}
