using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Zenject;

public class PlayerStateMoving :PlayerState {
    private bool moving;
    private Settings _settings;
    private Player _player;
    private LevelManager.Settings _levelSettings;

    public PlayerStateMoving(Settings settings, Player player, LevelManager.Settings levelSettings) {
        _settings = settings;
        _player = player;
        _levelSettings = levelSettings;
    }

    public override void Start() {
        moving = false;
    }

    public override void Update() {
        Move();
    }

    private void Move() {
        if(moving) {
            return;
        }

        if(Input.GetKeyDown(KeyCode.A)) {
            Move(Direction.LEFT);
        }
        if(Input.GetKeyDown(KeyCode.D)) {
            Move(Direction.RIGHT);
        }
        if(Input.GetKeyDown(KeyCode.W)) {
            Move(Direction.UP);
        }
        if(Input.GetKeyDown(KeyCode.S)) {
            Move(Direction.DOWN);
        }
    }

    private void Move(Direction direction) {
        Rotate(direction);
        switch(direction) {
            case Direction.LEFT:
                Move(Vector2.left);
                break;
            case Direction.RIGHT:
                Move(Vector2.right);
                break;
            case Direction.UP:
                Move(Vector2.up);
                break;
            case Direction.DOWN:
                Move(Vector2.down);
                break;
        }
    }

    private void Move(Vector2 position) {
        moving = true;
        _player.transform.DOMove((Vector2)_player.transform.position + (position * _levelSettings.gridSpace), _settings.moveSpeed).OnComplete(delegate () {
            moving = false;
        });
    }

    private void Rotate(Direction direction) {
        float angle = 0;
        switch(direction) {
            case Direction.LEFT:
                angle = 90;
                break;
            case Direction.RIGHT:
                angle = -90;
                break;
            case Direction.UP:
                angle = 0;
                break;
            case Direction.DOWN:
                angle = 180;
                break;
        }
        Rotate(angle);
    }

    private void Rotate(float angle) {
        _player.transform.DORotate(new Vector3(0, 0, angle), _settings.rotateSpeed);
    }

    enum Direction {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    [System.Serializable]
    public class Settings {
        public float moveSpeed;
        public float rotateSpeed;
    }

    public class Factory :PlaceholderFactory<PlayerStateMoving> {
    }
}
