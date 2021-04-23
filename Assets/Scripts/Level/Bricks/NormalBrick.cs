using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBrick :BaseBrick {

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Player")) {
            DestroyBrick();
            _signalBus.Fire<BrickDestroyedSignal>(new BrickDestroyedSignal { data = brickData });
        }
    }
}
