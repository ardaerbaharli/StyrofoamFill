using UnityEngine;

public class ProductionLineTile : MonoBehaviour
{
    public float slideSpeed;
    private void Start()
    {
        GetComponent<MeshRenderer>().material.color = new Color(0.16f, 0.16f, 0.16f);
    }
    private void Update()
    {
        transform.position += slideSpeed * Time.deltaTime * Vector3.right;
    }
}
