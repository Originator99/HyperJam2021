using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class HUDCanvas : MonoBehaviour {
    public GameObject radarPattern;
    public DisplayScore score;
    public Button quitBtn;

    private SignalBus _signalBus;

    [Inject]
    public void Construct(SignalBus signalBus) {
        _signalBus = signalBus;
    }

    private void Start() {
        quitBtn.onClick.RemoveAllListeners();
        quitBtn.onClick.AddListener(delegate () {
            _signalBus.Fire<PlayerReachedEndSignal>(new PlayerReachedEndSignal { hasWon = false});
        });
    }

    public void ShowHUD() {
        gameObject.SetActive(true);
    }

    public void HideHUD() {
        gameObject.SetActive(false);
    }

}
