using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Zenject;
using System;

public class CameraStateFollowing :CameraState {
    private readonly SignalBus _signalBus;
    private readonly Transform _bounds;
    private readonly CinemachineVirtualCamera _playerCamera;
    private readonly Camera _uiCamera;

    public CameraStateFollowing(SignalBus signalBus, [Inject(Id = "PlayerCamera")] Transform bounds, [Inject(Id = "UICamera")] Camera uiCamera, [Inject(Id = "PlayerCamera")] CinemachineVirtualCamera playerCamera) {
        _playerCamera = playerCamera;
        _uiCamera = uiCamera;
        _signalBus = signalBus;
        _bounds = bounds;

        _signalBus.Subscribe<LevelStartedSignal>(OnLevelStarted);
    }

    public override void LateUpdate() {
        _uiCamera.orthographicSize = _playerCamera.m_Lens.OrthographicSize;
    }

    public override void UpdateState(System.Object data) {

    }

    public override void Start() {
    }

    private void OnLevelStarted(LevelStartedSignal signalData) {
        if(signalData.levelSettings != null) {
            SetCameraBounds(signalData.levelSettings.worldSize, signalData.levelSettings.gridSpace);
        }
    }

    private void SetCameraBounds(Vector3 worldSize, float gridSpace) {
        if(_bounds != null) {
            _bounds.localScale = new Vector3(worldSize.x + 2f, worldSize.y + 2f);
        }
    }

    public class Factory :PlaceholderFactory<CameraStateFollowing> {

    }
}
