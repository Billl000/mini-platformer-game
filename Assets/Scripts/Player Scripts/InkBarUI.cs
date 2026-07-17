using UnityEngine;
using UnityEngine.UI;
public class InkBarUI : MonoBehaviour
{
    [SerializeField] private Ink inkCapacity;
    private Slider slider;
    private TMPro.TextMeshProUGUI inkText;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        inkText = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        //inkText.text = $"{inkCapacity.GetCurrentInk()}/{inkCapacity.GetMaxInk()}";
    }

    private void Start()
    {
        slider.value = 0f;

        inkCapacity.OnInkChanged += UpdateInkBar;
    }

    void UpdateInkBar(float value)
    {
        slider.value = value;
        //inkText.text = $"{inkCapacity.GetCurrentInk()}/{inkCapacity.GetMaxInk()}";
    }

    void OnDestroy()
    {
        slider.value = 0f;
        inkCapacity.OnInkChanged -= UpdateInkBar;
    }
}
