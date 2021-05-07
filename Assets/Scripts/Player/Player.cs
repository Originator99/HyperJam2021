using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class Player :MonoBehaviour {
    private AudioSource audioSource;

    private PlayerStateFactory _stateFactory;
    private PlayerState _state;

    public Direction currentDirection;

    public Dictionary<string, BaseBrick> dashSequence;

    public BaseBrick currentBrickCell;

    [Inject]
    public void Construct(PlayerStateFactory stateFactory) {
        _stateFactory = stateFactory;

        currentDirection = Direction.UP;
    }

    private void Start() {
        audioSource = GetComponent<AudioSource>();
        dashSequence = new Dictionary<string, BaseBrick>();
    }

    private void Update() {
        _state?.Update();
    }

    private void FixedUpdate() {
        _state?.FixedUpdate();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        _state?.OnTriggerEnter2D(collision);
    }

    public void ResetPlayerPosition(BaseBrick brickCell) {
        if(gameObject != null) {
            if(!gameObject.activeSelf) {
                gameObject.SetActive(true);
            }
            brickCell.SwitchToPath();
            currentBrickCell = brickCell;
            transform.position = brickCell.transform.position;
            if(dashSequence == null) {
                dashSequence = new Dictionary<string, BaseBrick>();
            } else {
                dashSequence.Clear();
            }
            ChangeState(PlayerStates.WaitingToStart);
        }
    }

    public void ChangeState(PlayerStates state) {
        if(_state != null) {
            _state.Dispose();
            _state = null;
        }
        Debug.Log("Changing to player state : " + state.ToString());
        _state = _stateFactory.CreateState(state);
        _state.Start();
    }

    public void ResetDash() {
        if(dashSequence == null) {
            dashSequence = new Dictionary<string, BaseBrick>();
        } else {
            foreach(var brick in dashSequence) {
                brick.Value.ToggleSelectState(false);
            }
            dashSequence.Clear();
        }
    }

    public void ModifyDashSequence(BaseBrick brick, out BaseBrick lastBrick) {
        lastBrick = brick;
        if(dashSequence != null) {
            //checking if sequence already has the brick in sequence or not. If not then we will add it to sequence
            if(!dashSequence.ContainsKey(brick.ID) && brick != null) {
                brick.ToggleSelectState(true);
                dashSequence.Add(brick.ID, brick);
                Debug.Log("Added to seq brick ID : " + brick.ID);
            } else {
                //if brick is already present then we want to remove it from the sequence if user selected the brick again
                List<string> toRemove = new List<string>();
                //finding all the bricks from the end of the sequence to the point user has touched
                foreach(KeyValuePair<string, BaseBrick> pair in dashSequence.Reverse()) {
                    if(pair.Key != brick.ID) {
                        toRemove.Add(pair.Key);
                    }
                    if(pair.Key == brick.ID) {
                        toRemove.Add(pair.Key);
                        break;
                    }
                }
                //removing it from the sequence
                foreach(string key in toRemove) {
                    dashSequence[key].ToggleSelectState(false);
                    dashSequence.Remove(key);
                    Debug.Log("Brick removed with ID : " + key);
                }

                //last brick is needed to find the next adjacent bricks to avoid adjacent movements
                if(dashSequence.Count > 0) {
                    lastBrick = dashSequence[dashSequence.Keys.Last()];
                } else {
                    //if all the items were removed then we want the current cell player is in to be returned
                    lastBrick = currentBrickCell;
                }
            }
        }
    }

    public void PlaySFX(AudioClip clip) {
        if(audioSource == null)
            audioSource = GetComponent<AudioSource>();
        if(clip != null && audioSource != null) {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
