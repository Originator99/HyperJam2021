using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CameraStateFollowing :CameraState {
    private readonly Player _player;
    private readonly CameraController _camera;
    private readonly Settings _settings;
    private readonly LevelHelper _levelHelper;

    private float leftBound, rightBound, bottomBound, topBound;

    private struct Bounds {
        public float minX;
        public float maxX;
        public float minY;
        public float maxY;
    }
    private Bounds bounds;

    public CameraStateFollowing(Settings settings, CameraController camera, Player player, LevelHelper levelHelper, LevelManager.Settings levelSettings) {
        _player = player;
        _camera = camera;
        _settings = settings;
        _levelHelper = levelHelper;

        bounds = new Bounds {
            minX = -(levelSettings.worldSize.x / 2),
            maxX = (levelSettings.worldSize.x / 2),
            minY = -(levelSettings.worldSize.y / 2),
            maxY = (levelSettings.worldSize.y / 2)
        };
    }

    public override void Start() {
        leftBound = bounds.minX - _levelHelper.ExtentWidth;
        rightBound = (bounds.maxX + _levelHelper.ExtentWidth) + 1f;
        bottomBound = bounds.minY - _levelHelper.ExtentHeight;
        topBound = (bounds.maxY + _levelHelper.ExtentHeight) + 1f;

        Debug.Log(topBound);
    }

    public override void LateUpdate() {
        if(_camera != null && _player != null) {
            float camX = Mathf.Clamp(_player.transform.position.x + 0.5f, leftBound, rightBound);
            float camY = Mathf.Clamp(_player.transform.position.y + 0.5f, bottomBound, topBound);

            Vector3 newPos = new Vector3(camX, camY);
            newPos.z = -10;

            if(bounds.maxX>_levelHelper.ExtentHeight) {

                _camera.transform.position = Vector3.Slerp(_camera.transform.position, newPos, _settings.speed * Time.deltaTime);
            }
        }

    }

    [System.Serializable]
    public class Settings {
        public float speed;
    }
    public class Factory :PlaceholderFactory<CameraStateFollowing> {

    }
}
