using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GaugeUI : MonoBehaviour
{
    private UIDocument _uiDocument;
    private VisualElement _root;
    private VisualElement _gaugeBarElem;
    private Label _waveLabel;
    

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
        _root = _uiDocument.rootVisualElement;
        _gaugeBarElem = _root.Q<VisualElement>("GaugeBar");
        _waveLabel = _root.Q<Label>("WaveRoundLabel");
    }

    public void UpdateGauge(float fillAmount)
    {
        var newStyleWidth = new StyleLength(fillAmount * 1070f);
        _gaugeBarElem.style.width = newStyleWidth;
    }

    public void UpdateWaveRound(int round)
    {
        _waveLabel.text = $"Wave: {round}";
    }
}
