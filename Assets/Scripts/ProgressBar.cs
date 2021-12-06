using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    private Slider slider;
    private float targetProgress = 0;
    public float fillSpeed;
    private float currentValue;
    private void Awake()
    {
        slider = GetComponent<Slider>();
    }
    private void Start()
    {
        fillSpeed = 3;
        currentValue = targetProgress;
    }
    public void IncrementValue(float newProgress)
    {
        currentValue += newProgress;
        targetProgress = currentValue;
    }

    private void Update()
    {
        if (slider.value < targetProgress)
            slider.value += fillSpeed;
    }
}
