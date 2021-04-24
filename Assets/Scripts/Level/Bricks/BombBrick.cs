using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BombBrick : BaseBrick {
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Player") && currentType == BrickType.BOMB) {
            _signalBus.Fire<PlayerDiedSignal>(new PlayerDiedSignal { });
        }
    }
}
