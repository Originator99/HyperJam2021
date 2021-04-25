using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BombBrick : BaseBrick {
    private bool isPlayerImmortal;

    private void Start() {
        isPlayerImmortal = false;
        _signalBus.Subscribe<PowerUpActivated>(OnPowerUpActivated);
        _signalBus.Subscribe<PowerUpDeactivated>(OnPowerUpDeactivated);
    }

    private void OnPowerUpActivated(PowerUpActivated signalData) {
        if(signalData.type == PowerUpType.IMMORTALITY) {
            isPlayerImmortal = true;
        }
    }

    private void OnPowerUpDeactivated(PowerUpDeactivated signalData) {
        if(signalData.type == PowerUpType.IMMORTALITY) {
            isPlayerImmortal = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Player") && currentType != BrickType.PATH) {
            if(currentType == BrickType.BOMB && !isPlayerImmortal) {
                _signalBus.Fire<PlayerDiedSignal>(new PlayerDiedSignal { });
            } else {
                _signalBus.Fire<BrickDestroyedSignal>(new BrickDestroyedSignal { data = brickData });
                DestroyBrick();
            }
        }
    }
}
