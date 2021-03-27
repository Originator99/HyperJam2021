using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using DG.Tweening;

public class PlayerStateDash :PlayerState {
    private readonly Settings _settings;
    private readonly Player _player;
    private readonly LevelManager.Settings _levelSettings;

    private bool dashing;
    private Sequence tweenSequence;

    public PlayerStateDash(Settings settings, Player player, LevelManager.Settings levelSettings) {
        _settings = settings;
        _player = player;
        _levelSettings = levelSettings;

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
            switch(_player.currentDirection) {
                case Direction.LEFT:
                    Dash(Vector2.left);
                    break;
                case Direction.RIGHT:
                    Dash(Vector2.right);
                    break;
                case Direction.UP:
                    Dash(Vector2.up);
                    break;
                case Direction.DOWN:
                    Dash(Vector2.down);
                    break;
            }
        }
    }

    private void Dash(Vector2 direction) {
        dashing = true;
        tweenSequence.Append(_player.transform.DOMove((Vector2)_player.transform.position + (direction * _levelSettings.gridSpace), _settings.dashSpeed).OnComplete(delegate () {
            dashing = false;
            _player.ChangeState(PlayerStates.Moving);
        }));
    }

    [System.Serializable]
    public class Settings {
        public float dashSpeed;
    }

    public class Factory :PlaceholderFactory<PlayerStateDash> {
    }
}
