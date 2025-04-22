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
    public void Start()
    {
        AdjustSize();
        AdjustGravity();
    }
    public void AdjustSize()
    {
        circlePrefab.transform.localScale = new(sizeSlider.value, sizeSlider.value, 1);
    }
    public void AdjustGravity()
    {
        gravityScript.G = gSlider.value;
    }
}
