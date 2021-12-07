using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    private Slider slider;
    private Image fillerImage;
    [SerializeField] private float targetProgress;
    public float fillSpeed;
    [SerializeField] private float currentValue;

    public void ResetComponent()
    {
        slider.value = 0;
        targetProgress = 0;
        fillerImage.color = Color.yellow;
        fillSpeed = 5;
        currentValue = targetProgress;
    }
    private void Awake()
    {
        slider = GetComponent<Slider>();
        fillerImage = transform.GetChild(1).GetChild(0).GetComponent<Image>();

    }
    private void Start()
    {
        ResetComponent();
    }
    public void IncrementValue(float newProgress)
    {
        currentValue += newProgress;
        targetProgress = currentValue;
        if (currentValue >= slider.maxValue)
            fillerImage.color = Color.red;
        else if (currentValue > slider.maxValue * 9 / 10)
            fillerImage.color = Color.green;
    }

    private void Update()
    {
        if (slider.value < targetProgress)
        {
            slider.value += fillSpeed;
        }
    }
}
