using UnityEngine;

public class LeftCollider : MonoBehaviour
{
    [SerializeField] private GameObject progressBar;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("ProgressBarCollider"))
        {
            var box = other.transform.parent;
            var objects = box.GetChild(box.childCount - 1);
            float totalVolume = 0;
            for (int i = 0; i < objects.childCount; i++)
            {
                totalVolume += objects.GetChild(i).GetComponent<BoxObject>().Volume;
            }
            progressBar.GetComponent<ProgressBar>().IncrementValue(totalVolume);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Equals("ProgressBarCollider"))
        {
            var box = other.transform.parent;
            box.GetComponent<Box>().OutOfNozzleRange();
            progressBar.GetComponent<ProgressBar>().ResetComponent();
        }

    }
}
