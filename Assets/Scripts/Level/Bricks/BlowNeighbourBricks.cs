using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowNeighbourBricks : BaseBrick {
    public float Range = 3f;
    public LayerMask brickLayerMask;
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Player")) {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, Range, brickLayerMask);
            if(colliders != null && colliders.Length >0) {
                for(int i = 0; i < colliders.Length; i++) {
                    BaseBrick brick = colliders[i].GetComponent<BaseBrick>();
                    if(brick != null && brick.currentType != BrickType.END) {
                        brick.DestroyBrick();
                    }
                }
            }
            DestroyBrick();
        }
    }
}
