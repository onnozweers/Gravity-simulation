using UnityEngine;
using UnityEngine.UI;

public class Sliders : MonoBehaviour
{
    [Header("Size Slider")]
    [SerializeField] Slider sizeSlider;
    [SerializeField] GameObject circlePrefab;
    [Header("Gravity Slider")]
    [SerializeField] Slider gSlider;
    [SerializeField] Gravity gravityScript;
    [Header("Time Slider")]
    [SerializeField] Slider timeSlider;
    private float timeSpeed = 1f;
    public void Start()
    {
        AdjustSize();
        AdjustGravity();
    }
    public void AdjustSize()
    {
        Debug.Log(sizeSlider.value);
        circlePrefab.transform.localScale = new(sizeSlider.value, sizeSlider.value, 1);
    }
    public void AdjustGravity()
    {
        Debug.Log(gSlider.value);
        gravityScript.G = gSlider.value;
    }
    public void AdjustTime()
    {
        Debug.Log(timeSlider.value);
        timeSpeed = timeSlider.value;
        Time.timeScale = timeSpeed;
    }
}
