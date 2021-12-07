using UnityEngine;

public class ProductionLineTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("tile"))
        {
            Destroy(other.gameObject);
        }
    }
}
