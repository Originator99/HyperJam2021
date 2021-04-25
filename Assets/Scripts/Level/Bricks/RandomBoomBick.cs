using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomBoomBick : BaseBrick {
    public LayerMask brickLayerMask;
    public int range = 5;
    public int randomBricksToBlow = 3;

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Player") && currentType != BrickType.PATH) {
            DestroyBrick();
            _signalBus.Fire<BrickDestroyedSignal>(new BrickDestroyedSignal { data = brickData });

            DoRandomBoomSequence();
        }
    }
    private void DoRandomBoomSequence() {
        Collider2D[] bricksInView = GetBricksInRange();
        if(bricksInView != null && bricksInView != null) {
            List<BaseBrick> bricks = FetchModifiedBrickList(bricksInView);
            BlowRandomBricks(bricks);
        }
    }

    /// <summary>
    /// Fetches a list of brick colliders from a point
    /// </summary>
    /// <returns></returns>
    private Collider2D[] GetBricksInRange() {
        return Physics2D.OverlapCircleAll(transform.position, range, brickLayerMask);
    }

    /// <summary>
    /// Removes bricks which are not supposed to be blown
    /// </summary>
    /// <param name="bricksInView">List of brick colliders</param>
    /// <returns></returns>
    private List<BaseBrick> FetchModifiedBrickList(Collider2D[] bricksInView) {
        List<BaseBrick> bricks = new List<BaseBrick>();
        for(int i = 0; i < bricksInView.Length; i++) {
            BaseBrick brick = bricksInView[i].GetComponent<BaseBrick>();
            if(brick != null && brick.currentType != BrickType.END && brick.currentType != BrickType.PATH) {
                bricks.Add(brick);
            }
        }
        return bricks;
    }

    /// <summary>
    /// Picks randomly x amount of bricks to blow in the level
    /// </summary>
    /// <param name="bricks">List of bricks to radomly blow from</param>
    private void BlowRandomBricks(List<BaseBrick> bricks) {
        for(int i = 0; i < randomBricksToBlow; i++) {
            if(bricks.Count > 0) {
                int index = Random.Range(0, bricks.Count);
                if(index < bricks.Count) {
                    bricks[index].DestroyBrick();
                    bricks.RemoveAt(index);
                }
            }
        }
    }
}
