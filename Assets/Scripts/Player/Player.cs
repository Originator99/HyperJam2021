using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Player :MonoBehaviour {
    private AudioSource audioSource;

    private PlayerStateFactory _stateFactory;
    private PlayerState _state;

    public Direction currentDirection;

    public Dictionary<string, Brick> dashSequence;

    public Brick currentBrickCell;

    [Inject]
    public void Construct(PlayerStateFactory stateFactory) {
        _stateFactory = stateFactory;

        currentDirection = Direction.UP;
    }

    private void Start() {
        audioSource = GetComponent<AudioSource>();
        dashSequence = new Dictionary<string, Brick>();
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

    public void ResetPlayerPosition(Brick brickCell) {
        if(!gameObject.activeSelf) {
            gameObject.SetActive(true);
        }
        currentBrickCell = brickCell;
        transform.position = brickCell.WorldPosition;
        if(dashSequence == null) {
            dashSequence = new Dictionary<string, Brick>();
        } else {
            dashSequence.Clear();
        }
        ChangeState(PlayerStates.WaitingToStart);
    }

    public void ChangeState(PlayerStates state) {
        if(_state != null) {
            _state.Dispose();
            _state = null;
        }
        _state = _stateFactory.CreateState(state);
        _state.Start();
    }

    public void ResetDash() {
        if(dashSequence == null) {
            dashSequence = new Dictionary<string, Brick>();
        } else {
            dashSequence.Clear();
        }
    }

    public void AddToDashSequence(Brick brick) {
        if(!dashSequence.ContainsKey(brick.IDOnGrid) && brick != null) {
            dashSequence.Add(brick.IDOnGrid, brick);
            Debug.Log("Added to seq brick ID : " + brick.IDOnGrid);
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
