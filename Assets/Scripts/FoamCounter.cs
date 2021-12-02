using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoamCounter : MonoBehaviour
{
    private Transform box;

    private void Start()
    {
        box = transform.parent;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("foam"))
        {
            other.transform.SetParent(box);
            box.GetComponent<Box>().RemainingVolume -= other.GetComponent<Foam>().Volume;
        }
    }
}
