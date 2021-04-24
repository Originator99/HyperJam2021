using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BombBrick : BaseBrick {
        private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Player")) {
            _signalBus.Fire<PlayerDiedSignal>(new PlayerDiedSignal { });
        }
    }

    public override void DestroyBrick() {
        Debug.LogWarning("Tried destroying bomb brick, we dont want that yet");
        //we dont want to destroy bomb brick
    }
}
