using Code.Core.EventSystems;
using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

namespace Code.Combats.Onset
{
    public class OnsetVignette : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO uiChannel;

        private Volume volume;
        private Vignette vignette;
        private bool isVignetteEnabled = false;

        private void Awake()
        {
            uiChannel.AddListener<OnsetUIEvent>(ToggleVignette);
        }


        private void Start()
        {
            volume = GetComponent<Volume>();
            if (!volume.profile.TryGet(out vignette))
            {
                Debug.LogError("Vignette is null");
            }
        }

        private void OnDestroy()
        {
            uiChannel.RemoveListener<OnsetUIEvent>(ToggleVignette);
        }


        private void ToggleVignette(OnsetUIEvent evt)
        {
            isVignetteEnabled = !isVignetteEnabled;
            float targetIntensity = isVignetteEnabled ? 0.6f : 0f;
            vignette.active = isVignetteEnabled;
            //vignette.intensity.Override(isVignetteEnabled ? 0.6f : 0f);
            //DOTween.To(() => vignette.intensity.value, x => vignette.intensity.value = x, targetIntensity, 0.25f)
            //       .SetEase(Ease.InOutQuad);
        }

    }
}

