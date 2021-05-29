using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using DG.Tweening;
using System;

public class ZoomUI :MonoBehaviour {
    public GameObject zoomPanel; 
    public Image zoomFill;
    public Button zoomBtn;

    private SignalBus _signalBus;
    private float autoZoomOutTimeLimit = 1;

    private bool isZoomedOut;

    [Inject]
    public void Construct(SignalBus signalBus, CameraStateZoom.Settings zoomSettings) {
        _signalBus = signalBus;
        autoZoomOutTimeLimit = zoomSettings.autoZoomInTime;

        _signalBus.Subscribe<LevelStartedSignal>(OnLevelStarted);
    }

    private void OnLevelStarted(LevelStartedSignal signalData) {
        if(signalData.levelSettings != null && signalData.levelSettings.worldSize.x > 25 && signalData.levelSettings.worldSize.y > 25) {
            zoomPanel.SetActive(true);
        } else {
            zoomPanel.SetActive(false);
        }
    }

    private void Start() {
        zoomBtn.onClick.RemoveAllListeners();
        zoomBtn.onClick.AddListener(delegate() {
            if(!isZoomedOut) {
                ZoomSequence();
            }
        });
    }

    private void ZoomSequence() {
        _signalBus.Fire<CameraZoomSingal>(new CameraZoomSingal { isZoom = true });
        isZoomedOut = true;
        zoomBtn.interactable = false;
        zoomFill.fillAmount = 1;
        zoomFill.DOFillAmount(0, autoZoomOutTimeLimit).OnComplete(delegate() {
            zoomBtn.interactable = true;
            isZoomedOut = false;
            _signalBus.Fire<CameraZoomSingal>(new CameraZoomSingal { isZoom = false });
        });
    }
}