
using UnityEngine;

public class MidScreenCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("ProgressBarCollider"))
        {
            var box = other.transform.parent;
           //var topRight =
        }
    }
}
