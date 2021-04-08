using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using DG.Tweening;

public class RadarUI : MonoBehaviour {
    public CanvasGroup radarGFX;
    public Button radarButton;

    private Radar _radar;

    [Inject]
    public void Construct(Radar radar) {
        _radar = radar;

        _radar.OnRadarReady += HandleRadarReady;
    }

    private void Start() {
        radarButton.onClick.RemoveAllListeners();
        radarButton.onClick.AddListener(delegate() {
            if(_radar.CanActivateRadar()) {
                _radar.ActivateRadar();
                SetAsInactive();
            }
        });

        SetAsInactive();
    }

    private void HandleRadarReady() {
        SetAsActive();
    }

    private void SetAsActive() {
        radarGFX.DOFade(1f, 0.25f);
    }

    private void SetAsInactive() {
        radarGFX.DOFade(0.5f, 0.25f);
    }
}
