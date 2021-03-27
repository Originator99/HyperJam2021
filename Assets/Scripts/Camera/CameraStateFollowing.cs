using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CameraStateFollowing :CameraState {
    private readonly Player _player;
    private readonly CameraController _camera;
    private readonly Settings _settings;

    public CameraStateFollowing(Settings settings, CameraController camera, Player player) {
        _player = player;
        _camera = camera;
        _settings = settings;
    }

    public override void LateUpdate() {
        if(_camera != null && _player != null) {
            Vector3 newPos = _player.transform.position + new Vector3(0.5f, 0.5f, 0f);
            newPos.z = -10;
            _camera.transform.position = Vector3.Slerp(_camera.transform.position, newPos, _settings.speed * Time.deltaTime);
        }
    }

    [System.Serializable]
    public class Settings {
        public float speed;
    }
    public class Factory :PlaceholderFactory<CameraStateFollowing> {

    }
}
