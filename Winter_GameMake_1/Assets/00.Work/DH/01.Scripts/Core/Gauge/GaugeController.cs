using UnityEngine;

public class GaugeController : MonoBehaviour
{
    [SerializeField] private GaugeData gaugeData;
    [SerializeField] private GaugeUI gaugeUI;

    private void OnEnable()
    {
        gaugeData.OnGaugeValueChanged += HandleGaugeChange;
    }

    private void OnDisable()
    {
        gaugeData.OnGaugeValueChanged -= HandleGaugeChange;
    }
    
    private void HandleGaugeChange(float value)
    {
        float fillAmount = gaugeData.GaugeValue / gaugeData.maxValue;
        gaugeUI.UpdateGauge(fillAmount);
    }
}
