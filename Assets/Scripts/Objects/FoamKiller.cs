using UnityEngine;

public class FoamKiller : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
    }
}
