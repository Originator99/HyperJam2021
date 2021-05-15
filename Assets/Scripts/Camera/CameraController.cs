using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CameraController : MonoBehaviour {
    private CameraStateFactory _cameraStateFactory;
    private CameraState _cameraState;
    private SignalBus _signalBus;

    private CameraState.State currentState;

    [Inject]
    public void Construct(SignalBus signalBus, CameraStateFactory cameraStateFactory) {
        _signalBus = signalBus;
        _cameraStateFactory = cameraStateFactory;

        _signalBus.Subscribe<CameraZoomSingal>(OnCameraZoomSignal);

        currentState = CameraState.State.Static;
    }

    private void OnCameraZoomSignal(CameraZoomSingal signalData) {
        ChangeState(CameraState.State.Zoom, signalData);
    }

    private void Start() {
        ChangeState(CameraState.State.Following);
    }
    private void LateUpdate() {
        if(_cameraState != null) {
            _cameraState.LateUpdate();
        }
    }

    public void ChangeState(CameraState.State state, System.Object data = null) {
        if(_cameraState != null && currentState != state) {
            _cameraState.Dispose();
            _cameraState = null;
        }

        if(currentState != state && _cameraState == null) {
            _cameraState = _cameraStateFactory.CreateFactory(state);
        }

        _cameraState.UpdateState(data);
        currentState = state;
    }
}
