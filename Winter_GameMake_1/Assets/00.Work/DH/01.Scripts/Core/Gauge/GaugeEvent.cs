using UnityEngine;

public class GaugeEvent : MonoBehaviour
{
    [SerializeField] private float eventThreshold;

    /*private void Update()
    {
        if (GaugeManager.Instance == null) return;

        float currentFill = GaugeManager.Instance.OnGaugeChanged?.GetInvocationList()
            .Length ?? 0;

        if (currentFill < eventThreshold)
        {
            Debug.Log("Max Value");
        }
    }*/
}
