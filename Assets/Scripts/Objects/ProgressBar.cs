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
        fillSpeed = 6f;
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
    }

    private void Update()
    {
        if (slider.value < targetProgress)
        {
            slider.value += fillSpeed;
        }

        if (slider.value >= slider.maxValue)
            fillerImage.color = Color.red;
        else if (slider.value > slider.maxValue * 0.9f)
            fillerImage.color = Color.green;
    }
}
