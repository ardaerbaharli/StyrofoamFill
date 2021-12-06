using UnityEngine;

public class FoamCounter : MonoBehaviour
{
    private ProgressBar progressBar;
    private Transform box;

    private void Start()
    {
        progressBar = FindObjectOfType<ProgressBar>();
        box = transform.parent;
        float totalItemVolume = 0;
        var objHolder = box.GetChild(box.childCount - 1);
        for (int i = 0; i < objHolder.childCount; i++)
        {
            totalItemVolume += objHolder.GetChild(i).GetComponent<BoxObject>().Volume;
        }
        AddedItem(totalItemVolume);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("foam"))
        {
            other.transform.SetParent(box);
            var foamVolume = other.GetComponent<Foam>().Volume;
            AddedItem(foamVolume);
        }
    }

    private void AddedItem(float volume)
    {
        progressBar.IncrementValue(volume);
        box.GetComponent<Box>().RemainingVolume -= volume;
    }
}
