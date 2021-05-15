using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Cinemachine;
using DG.Tweening;

public class CameraStateZoom :CameraState {
    private Settings _settings;
    private CinemachineVirtualCamera _camera;

    private bool isZooming;

    public CameraStateZoom([Inject(Id = "Camera")] CinemachineVirtualCamera camera, Settings settings) {
        _camera = camera;
        _settings = settings;

        isZooming = false;
    }


    public override void LateUpdate() {
        if(isZooming) {
            if(_camera.m_Lens.OrthographicSize <= _settings.zoomOutOrthoSize) {
                _camera.m_Lens.OrthographicSize += 1f * _settings.zoomSpeed * Time.deltaTime;
            }
        } else {
            if(_camera.m_Lens.OrthographicSize >= _settings.originalOrthoSize) {
                _camera.m_Lens.OrthographicSize -= 1f * _settings.zoomSpeed * Time.deltaTime;
            }
        }
    }

    public override void UpdateState(System.Object data) {
        if(data != null && data.GetType() == typeof(CameraZoomSingal)) {
            SwitchZoom((CameraZoomSingal)data);
        }
    }

    private void SwitchZoom(CameraZoomSingal data) {
        if(data.isZoom && _camera.m_Lens.OrthographicSize <= _settings.originalOrthoSize) {
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
    }

    public class Factory :PlaceholderFactory<CameraStateZoom> {

    }
}
