using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using DG.Tweening;

public class ZoomUI :MonoBehaviour {
    public Image zoomFill;
    public Button zoomBtn;

    private SignalBus _signalBus;
    private float autoZoomOutTimeLimit = 1;

    private bool isZoomedOut;

    [Inject]
    public void Construct(SignalBus signalBus, CameraStateZoom.Settings zoomSettings) {
        _signalBus = signalBus;
        autoZoomOutTimeLimit = zoomSettings.autoZoomInTime;
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