using UnityEngine;

public class FoamCounter : MonoBehaviour
{
    private ProgressBar progressBar;
    public Transform foamHolder;

    private void Start()
    {
        progressBar = FindObjectOfType<ProgressBar>();
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("Foam"))
        {
            other.transform.SetParent(foamHolder);
            var foamVolume = other.GetComponent<Foam>().Volume;
            AddedItem(foamVolume);
        }
    }

    private void AddedItem(float volume)
    {
        progressBar.IncrementValue(volume);
        foamHolder.parent.GetComponent<Box>().RemainingVolume -= volume;
    }
}
