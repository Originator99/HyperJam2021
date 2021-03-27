﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using DG.Tweening;

public class PlayerStateDash :PlayerState {
    private readonly Settings _settings;
    private readonly Player _player;
    private readonly LevelManager.Settings _levelSettings;
    private readonly LevelManager _levelManager;

    private bool dashing;
    private Sequence tweenSequence;

    public PlayerStateDash(Settings settings, Player player, LevelManager.Settings levelSettings, LevelManager levelManager) {
        _settings = settings;
        _player = player;
        _levelSettings = levelSettings;
        _levelManager = levelManager;

        tweenSequence = DOTween.Sequence();
        dashing = false;
    }

    public override void Start() {
        StartDash();
    }

    public override void OnTriggerEnter2D(Collider2D collider) {
        if(collider != null && collider.gameObject != null) {
            if(collider.CompareTag("Brick") && dashing) {
                Brick controller = collider.GetComponent<Brick>();
                if(controller != null) {
                    if(controller.currentType == BrickType.NORMAL) {
                        controller.DestroyBrick();
                    }
                }
            }
        }
    }

    public override void Update() {

    }

    private void StartDash() {
        if(!dashing) {
            Dash(_player.currentDirection);
        }
    }

    private void Dash(Direction direction) {
        Node2D nextBrickCell = _levelManager.GetBrickInDirection(direction, _player.currentBrickCell);
        if(nextBrickCell != null) {
            dashing = true;
            _player.currentBrickCell = nextBrickCell;
            tweenSequence.Append(_player.transform.DOMove((Vector2)nextBrickCell.worldPosition, _settings.dashSpeed).OnComplete(delegate () {
                dashing = false;
                _player.ChangeState(PlayerStates.Moving);
            }));
        } else {
            _player.ChangeState(PlayerStates.Moving);
        }
    }

    [System.Serializable]
    public class Settings {
        public float dashSpeed;
    }

    public class Factory :PlaceholderFactory<PlayerStateDash> {
    }
}
