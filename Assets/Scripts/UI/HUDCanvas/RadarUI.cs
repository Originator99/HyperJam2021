using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using DG.Tweening;
using TMPro;

public class RadarUI : MonoBehaviour {
    public CanvasGroup radarGFX;
    public Button radarButton;
    public TMP_Text totalCount;

    private Radar _radar;

    [Inject]
    public void Construct(Radar radar) {
        _radar = radar;

        _radar.OnRadarReady += HandleRadarReady;
        _radar.OnRadarNotReady += HandleRadarNotReady;
    }

    private void Start() {
        radarButton.onClick.RemoveAllListeners();
        radarButton.onClick.AddListener(delegate() {
            if(_radar.CanActivateRadar()) {
                _radar.ActivateRadar();
            }
        });
    }

    private void HandleRadarReady() {
        totalCount.text = _radar.GetTotalRadars().ToString();
        SetAsActive();
    }
    private void HandleRadarNotReady() {
        totalCount.text = _radar.GetTotalRadars().ToString();
        SetAsInactive();
    }

    private void SetAsActive() {
        radarGFX.DOFade(1f, 0.25f);
    }

    private void SetAsInactive() {
        radarGFX.DOFade(0.5f, 0.25f);
    }
}
