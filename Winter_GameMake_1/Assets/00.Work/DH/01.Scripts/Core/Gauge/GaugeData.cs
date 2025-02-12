using NUnit.Framework.Constraints;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GaugeData", menuName = "Gauge/GaugeData")]
public class GaugeData : ScriptableObject
{
    public Action<float> OnGaugeValueChanged;
    public float maxValue = 1f;

    private float gaugeValue = 1f;
    public float GaugeValue
    {
        get => gaugeValue;

        set
        {
            if (gaugeValue == value) return;

            gaugeValue = Mathf.Clamp(value, 0, maxValue);
            OnGaugeValueChanged?.Invoke(gaugeValue);
        }
    }
}
