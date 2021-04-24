using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Zenject;
using UnityEngine.EventSystems;

public class PlayerStateMoving :PlayerState {
    private bool rotating;
    private Settings _settings;
    private Player _player;
    private LevelManager _levelManager;
    private readonly SignalBus _signalBus;

    private float waitForDash; //going to use this variable to check touch time of a user. We dont want to dash as soon as touch
    private List<BaseBrick> currentAdjacentBricks;
    private BaseBrick currentBrickInTouch;
    private int brickLayerMask;

    public PlayerStateMoving(Settings settings, Player player, LevelManager levelManager, SignalBus signalBus) {
        _settings = settings;
        _player = player;
        _signalBus = signalBus;
        _levelManager = levelManager;

        _signalBus.Subscribe<PlayerInputSignal>(OnPlayerInput);

        brickLayerMask = 1 << LayerMask.NameToLayer("Brick");
    }

    public override void Start() {
        _player.gameObject.SetActive(true);
    }

    public override void Update() {
        //if(!EventSystem.current.IsPointerOverGameObject()) {
        //    if(Input.GetMouseButtonDown(0)) {
        //        waitForDash = _settings.touchTimeBeforeDash;
        //    }
        //    if(Input.GetMouseButton(0)) {
        //        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - _player.transform.position;
        //        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //        _player.transform.rotation = Quaternion.Slerp(_player.transform.rotation, rotation, 15 * Time.unscaledDeltaTime);

        //        if(waitForDash > 0) {
        //            waitForDash -= Time.unscaledDeltaTime;
        //        }
        //    }
        //    if(Input.GetMouseButtonUp(0)) {
        //        RotateToClosest();
        //    }
        //}
        if(!EventSystem.current.IsPointerOverGameObject()) {
            if(Input.GetMouseButtonDown(0)) {
                RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.down, 2f);
                foreach(RaycastHit2D hit in hits) {
                    if(hit.collider != null && hit.collider.CompareTag("Player")) {
                        canDash = true;
                        GenerateAdjacentBricks(currentBrickInTouch = _player.currentBrickCell);
                        Debug.Log("Player Found, Staring to dash");
                    }
                }
                _player.ResetDash();
            }
            if(Input.GetMouseButton(0) && canDash) {
                Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - _player.transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                _player.transform.rotation = Quaternion.Slerp(_player.transform.rotation, rotation, 15 * Time.unscaledDeltaTime);

                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.down, 2f, brickLayerMask);
                if(hit.collider != null) {
                    BaseBrick brick = hit.collider.GetComponent<BaseBrick>();
                    if(brick != null && CheckIfAdjacentBrick(brick.ID, out BaseBrick adj)) {
                        _player.ModifyDashSequence(adj, out currentBrickInTouch);
                        GenerateAdjacentBricks(currentBrickInTouch);
                    }
                }
            }
            if(Input.GetMouseButtonUp(0)) {
                currentBrickInTouch = null;
                canDash = false;
                _player.ChangeState(PlayerStates.Dash);
            }
        }
    }

    bool canDash;
    public override void FixedUpdate() {
        
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
        Rotate(direction);
    }

    public override void Dispose() {
        _signalBus.Unsubscribe<PlayerInputSignal>(OnPlayerInput);
    }

    private void OnPlayerInput(PlayerInputSignal signalData) {
        if(signalData.doDash && !rotating) {

        }
        else if(!rotating && _player.currentDirection != signalData.moveDirection) {
            Rotate(signalData.moveDirection);
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

            if(waitForDash <= 0) {
                _player.ChangeState(PlayerStates.Dash);
            }
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

    private void GenerateAdjacentBricks(BaseBrick currentBrick) {
        if(currentAdjacentBricks == null)
            currentAdjacentBricks = new List<BaseBrick>();
        else
            currentAdjacentBricks.Clear();

        if(currentBrick != null) {
            BaseBrick brickLeft = _levelManager.GetBrickInDirectionFrom(currentBrick, Direction.LEFT, currentBrick.transform.position);
            if(brickLeft != null && brickLeft.currentType != BrickType.UNBREAKABLE) {
                currentAdjacentBricks.Add(brickLeft);
            }
            BaseBrick brickRight = _levelManager.GetBrickInDirectionFrom(currentBrick, Direction.RIGHT, currentBrick.transform.position);
            if(brickRight != null && brickRight.currentType != BrickType.UNBREAKABLE) {
                currentAdjacentBricks.Add(brickRight);
            }
            BaseBrick brickUp = _levelManager.GetBrickInDirectionFrom(currentBrick, Direction.UP, currentBrick.transform.position);
            if(brickUp != null && brickUp.currentType != BrickType.UNBREAKABLE) {
                currentAdjacentBricks.Add(brickUp);
            }
            BaseBrick brickDown = _levelManager.GetBrickInDirectionFrom(currentBrick, Direction.DOWN, currentBrick.transform.position);
            if(brickDown != null && brickDown.currentType != BrickType.UNBREAKABLE) {
                currentAdjacentBricks.Add(brickDown);
            }
        }
    }

    private bool CheckIfAdjacentBrick(string brickID, out BaseBrick brick) {
        brick = null;
        if(currentAdjacentBricks != null) {
            int index = currentAdjacentBricks.FindIndex(x => x.ID == brickID);
            if(index >= 0) {
                brick = currentAdjacentBricks[index];
                return true;
            }
        }
        return false;
    }

    [System.Serializable]
    public class Settings {
        public float rotateSpeed;
        public float touchTimeBeforeDash;

        public AudioClip rotateSFX;
    }

    public class Factory :PlaceholderFactory<PlayerStateMoving> {
    }
}
