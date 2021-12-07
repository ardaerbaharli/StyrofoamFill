using UnityEngine;

public class ProductionLineTile : MonoBehaviour
{
    private void Start()
    {
        GetComponent<MeshRenderer>().material.color = new Color(0.16f, 0.16f, 0.16f);
    }
    void Update()
    {
        transform.position += Vector3.right * 0.5f * Time.deltaTime;
    }
}
