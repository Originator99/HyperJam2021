﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Zenject;
using System;

public class PlayerStateMoving :PlayerState {
    private bool rotating;
    private Settings _settings;
    private Player _player;
    private readonly SignalBus _signalBus;
    private readonly Camera _camera;

    public PlayerStateMoving(Settings settings, Player player, SignalBus signalBus, [Inject(Id = "Main")] Camera camera) {
        _settings = settings;
        _player = player;
        _signalBus = signalBus;
        _camera = camera;

        _signalBus.Subscribe<PlayerInputSignal>(OnPlayerInput);
    }

    public override void Start() {
        _player.gameObject.SetActive(true);
    }

    public override void Update() {
        if(Input.GetMouseButton(0)) {
            Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - _player.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            _player.transform.rotation = Quaternion.Slerp(_player.transform.rotation, rotation, 15 * Time.unscaledDeltaTime);
        }
        if(Input.GetMouseButtonUp(0)) {
            RotateToClosest();
        }
    }

    private void RotateToClosest() {
        float z = _player.transform.rotation.eulerAngles.z;
        Direction direction = Direction.NONE;
        if(z >= 45 && z < 135f) {
            direction = Direction.UP;
        } else if(z >= 135f && z < 225f) {
            direction = Direction.LEFT;
        } else if(z >= 225 && z < 315f) {
            direction = Direction.DOWN;
        } else if(z > 315f || z < 45) {
            direction = Direction.RIGHT;
        }
        Debug.Log(z);
        Rotate(direction);
    }

    public override void Dispose() {
        _signalBus.Unsubscribe<PlayerInputSignal>(OnPlayerInput);
    }

    private void OnPlayerInput(PlayerInputSignal signalData) {
        Debug.Log("signal fired");
        if(signalData.doDash && !rotating) {

        }
        else if(!rotating && _player.currentDirection != signalData.moveDirection) {
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
                angle = -180;
                break;
            case Direction.RIGHT:
                angle = 0;
                break;
            case Direction.UP:
                angle = 90;
                break;
            case Direction.DOWN:
                angle = -90;
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
            _player.ChangeState(PlayerStates.Dash);
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
