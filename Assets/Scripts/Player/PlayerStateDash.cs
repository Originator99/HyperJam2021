using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using DG.Tweening;

public class PlayerStateDash :PlayerState {
    private readonly Settings _settings;
    private readonly Player _player;
    private readonly LevelManager _levelManager;
    private readonly SignalBus _signalBus;

    private bool dashing;
    private Sequence tweenSequence;

    public PlayerStateDash(Settings settings, Player player, LevelManager levelManager, SignalBus signalBus) {
        _settings = settings;
        _player = player;
        _levelManager = levelManager;
        _signalBus = signalBus;

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
                        _signalBus.Fire<BrickDestroyedSignal>(new BrickDestroyedSignal { data = controller.data });
                        _player.PlaySFX(_settings.destroyBrickSFX);
                    }
                    if(controller.currentType == BrickType.BOMB) {
                        tweenSequence.Kill();
                        _player.PlaySFX(_settings.dashSFX);
                        _signalBus.Fire<PlayerDiedSignal>(new PlayerDiedSignal { });
                    }
                    if(controller.currentType == BrickType.END) {
                        _signalBus.Fire<PlayerReachedEndSignal>(new PlayerReachedEndSignal { hasWon = true });
                        tweenSequence.Kill();
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
        Brick nextBrickCell = _levelManager.GetBrickInDirection(direction);
        if(nextBrickCell != null) {
            dashing = true;
            _player.currentBrickCell = nextBrickCell;
            _player.PlaySFX(_settings.dashSFX);
            tweenSequence.Append(_player.transform.DOMove(nextBrickCell.WorldPosition, _settings.dashSpeed).OnComplete(delegate () {
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
        public AudioClip dashSFX;
        public AudioClip destroyBrickSFX;
        public AudioClip deathSFX;
    }

    public class Factory :PlaceholderFactory<PlayerStateDash> {
    }
}
