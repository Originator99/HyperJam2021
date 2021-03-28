﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Player :MonoBehaviour {
    private AudioSource audioSource;

    private PlayerStateFactory _stateFactory;
    private PlayerState _state;

    public Direction currentDirection;
    public Node2D currentBrickCell;

    [Inject]
    public void Construct(PlayerStateFactory stateFactory) {
        _stateFactory = stateFactory;

        currentDirection = Direction.UP;
    }

    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update() {
        _state?.Update();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        _state?.OnTriggerEnter2D(collision);
    }

    public void ResetPlayerPosition(Node2D brickCell) {
        currentBrickCell = brickCell;
        transform.position = brickCell.worldPosition;
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
    
    public void PlaySFX(AudioClip clip) {
        if(audioSource == null)
            audioSource = GetComponent<AudioSource>();
        if(clip != null && audioSource != null) {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
