using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Zenject;

public class PlayerStateMoving :PlayerState {
    private bool rotating;
    private Settings _settings;
    private Player _player;
    private LevelManager.Settings _levelSettings;
    private readonly SignalBus _signalBus;

    public PlayerStateMoving(Settings settings, Player player, SignalBus signalBus, LevelManager.Settings levelSettings) {
        _settings = settings;
        _player = player;
        _levelSettings = levelSettings;
        _signalBus = signalBus;

        _signalBus.Subscribe<PlayerInputSignal>(OnPlayerInput);
    }

    public override void Start() {
        _player.gameObject.SetActive(true);
    }

    public override void Update() {
    }

    public override void Dispose() {
        _signalBus.Unsubscribe<PlayerInputSignal>(OnPlayerInput);
    }

    private void OnPlayerInput(PlayerInputSignal signalData) {
        Debug.Log("signal fired");
        if(signalData.doDash && !rotating) {
            _player.ChangeState(PlayerStates.Dash);
        }
        else if(!rotating) {
            Rotate(signalData.moveDirection);
        }
    }

    private void Rotate() {
        //Because i made this support and i wanted to use it somewhere
        if(Input.GetKey(KeyCode.LeftShift)) {
            DoNextRotation(Direction.LEFT);
        }
        if(Input.GetKey(KeyCode.RightShift)) {
            DoNextRotation(Direction.RIGHT);
        }
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
        _player.currentDirection = direction;
        Rotate(angle);
    }

    private void Rotate(float angle) {
        rotating = true;
        if(_player.transform.rotation.eulerAngles.z != angle) {
            _player.PlaySFX(_settings.rotateSFX);
        }
        _player.transform.DORotate(new Vector3(0, 0, angle), _settings.rotateSpeed).OnComplete(delegate () {
            rotating = false;
        });
    }

    private void DoNextRotation(Direction tryToRotateIn) {
        if(tryToRotateIn == Direction.LEFT) {
            if(_player.currentDirection == Direction.UP) {
                Rotate(Direction.LEFT);
            } else if(_player.currentDirection == Direction.LEFT) {
                Rotate(Direction.DOWN);
            } else if(_player.currentDirection == Direction.DOWN) {
                Rotate(Direction.RIGHT);
            } else if(_player.currentDirection == Direction.RIGHT) {
                Rotate(Direction.UP);
            }
        } else if(tryToRotateIn == Direction.RIGHT) {
            if(_player.currentDirection == Direction.UP) {
                Rotate(Direction.RIGHT);
            } else if(_player.currentDirection == Direction.RIGHT) {
                Rotate(Direction.DOWN);
            } else if(_player.currentDirection == Direction.DOWN) {
                Rotate(Direction.LEFT);
            } else if(_player.currentDirection == Direction.LEFT) {
                Rotate(Direction.UP);
            }
        } 
    }

    [System.Serializable]
    public class Settings {
        public float moveSpeed;
        public float rotateSpeed;

        public AudioClip rotateSFX;
    }

    public class Factory :PlaceholderFactory<PlayerStateMoving> {
    }
}

public enum Direction {
    NONE,
    UP,
    DOWN,
    LEFT,
    RIGHT
}
