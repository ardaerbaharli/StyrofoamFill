using UnityEngine;

public class Cleanup : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("Foam") || other.name.Equals("Reaction"))
            Destroy(other.gameObject);
    }
}
