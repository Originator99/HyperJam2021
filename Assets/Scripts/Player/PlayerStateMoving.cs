using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Zenject;

public class PlayerStateMoving :PlayerState {
    private bool moving, rotating;
    private Settings _settings;
    private Player _player;
    private LevelManager.Settings _levelSettings;

    public PlayerStateMoving(Settings settings, Player player, LevelManager.Settings levelSettings) {
        _settings = settings;
        _player = player;
        _levelSettings = levelSettings;

        moving = false;
    }

    public override void Start() {
    }

    public override void Update() {
        if(!moving) {
            //Move(); disabling movement for now
        }

        if(!rotating) {
            Rotate();
        }


        if(Input.GetKeyDown(KeyCode.Space) && !moving && !rotating) {
            _player.ChangeState(PlayerStates.Dash);
        }
    }

    private void Move() {
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

    private void Rotate() {
        if(Input.GetKeyDown(KeyCode.LeftArrow)) {
            Rotate(Direction.LEFT);
        }
        if(Input.GetKeyDown(KeyCode.RightArrow)) {
            Rotate(Direction.RIGHT);
        }
        if(Input.GetKeyDown(KeyCode.UpArrow)) {
            Rotate(Direction.UP);
        }
        if(Input.GetKeyDown(KeyCode.DownArrow)) {
            Rotate(Direction.DOWN);
        }

        //Because i made this support and i wanted to use it somewhere
        if(Input.GetKey(KeyCode.LeftShift)) {
            DoNextRotation(Direction.LEFT);
        }
        if(Input.GetKey(KeyCode.RightShift)) {
            DoNextRotation(Direction.RIGHT);
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
        _player.currentDirection = direction;
        Rotate(angle);
    }

    private void Rotate(float angle) {
        rotating = true;
        _player.transform.DORotate(new Vector3(0, 0, angle), _settings.rotateSpeed).OnComplete(delegate() {
            rotating = false;
        });
    }

    private void DoNextRotation(Direction tryToRotateIn) {
        if(tryToRotateIn == Direction.LEFT) {
            if(_player.currentDirection == Direction.UP) {
                Rotate(Direction.LEFT);
            }else if(_player.currentDirection == Direction.LEFT) {
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
    }

    public class Factory :PlaceholderFactory<PlayerStateMoving> {
    }
}

public enum Direction {
    UP,
    DOWN,
    LEFT,
    RIGHT
}
