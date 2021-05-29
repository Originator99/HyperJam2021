using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Cinemachine;
using DG.Tweening;

public class CameraStateZoom :CameraState {
    private readonly Settings _settings;
    private readonly CinemachineVirtualCamera _playerCamera;
    private readonly Camera _uiCamera;

    private bool isZooming;

    public CameraStateZoom([Inject(Id = "PlayerCamera")] CinemachineVirtualCamera playerCamera, [Inject(Id ="UICamera")] Camera uiCamera, Settings settings) {
        _playerCamera = playerCamera;
        _uiCamera = uiCamera;
        _settings = settings;

        isZooming = false;
    }


    public override void LateUpdate() {
        if(isZooming) {
            if(_playerCamera.m_Lens.OrthographicSize <= _settings.zoomOutOrthoSize) {
                _playerCamera.m_Lens.OrthographicSize += 1f * _settings.zoomSpeed * Time.deltaTime;
            }
        } else {
            if(_playerCamera.m_Lens.OrthographicSize >= _settings.originalOrthoSize) {
                _playerCamera.m_Lens.OrthographicSize -= 1f * _settings.zoomSpeed * Time.deltaTime;
            }
        }
        _uiCamera.orthographicSize = _playerCamera.m_Lens.OrthographicSize;
    }

    public override void UpdateState(System.Object data) {
        if(data != null && data.GetType() == typeof(CameraZoomSingal)) {
            SwitchZoom((CameraZoomSingal)data);
        }
    }

    private void SwitchZoom(CameraZoomSingal data) {
        if(data.isZoom && _playerCamera.m_Lens.OrthographicSize <= _settings.originalOrthoSize) {
            isZooming = true;
        } else {
            isZooming = false;
        }
    }

    [System.Serializable]
    public class Settings {
        public float originalOrthoSize;
        public float zoomOutOrthoSize;
        public float zoomSpeed;
        public float autoZoomInTime;
    }

    public class Factory :PlaceholderFactory<CameraStateZoom> {

    }
}
