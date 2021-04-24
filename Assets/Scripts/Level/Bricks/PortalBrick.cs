using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PortalBrick : BaseBrick {
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Player")) {
            _signalBus.Fire<PlayerReachedEndSignal>(new PlayerReachedEndSignal { hasWon = true });
        }
    }
}
