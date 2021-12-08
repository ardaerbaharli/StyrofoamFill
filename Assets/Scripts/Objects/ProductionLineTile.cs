using UnityEngine;

public class ProductionLineTile : MonoBehaviour
{
    public float slideSpeed;
    public bool slide;
    private void Start()
    {
        GetComponent<MeshRenderer>().material.color = new Color(0.16f, 0.16f, 0.16f);
        slide = true;
    }
    private void Update()
    {
        if (slide)
            transform.position += slideSpeed * Time.deltaTime * Vector3.right;
    }
}
