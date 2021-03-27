using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CameraController : MonoBehaviour {
    private CameraStateFactory _cameraStateFactory;
    private CameraState _cameraState;

    [Inject]
    public void Construct(CameraStateFactory cameraStateFactory) {
        _cameraStateFactory = cameraStateFactory;
    }


    private void Start() {
        //ChangeState(CameraState.State.Following);
    }
    private void LateUpdate() {
        if(_cameraState != null) {
            _cameraState.LateUpdate();
        }
    }

    public void ChangeState(CameraState.State state) {
        if(_cameraState != null) {
            _cameraState.Dispose();
            _cameraState = null;
        }
        _cameraState = _cameraStateFactory.CreateFactory(state);
        _cameraState.Start();
    }
}
