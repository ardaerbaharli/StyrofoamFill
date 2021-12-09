using UnityEngine;

public class Reaction : MonoBehaviour
{
    void Update()
    {
        if (GetComponent<RectTransform>().position.y < -100)
            Destroy(gameObject);
    }
}
