using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public enum Direction {
    NONE,
    UP,
    DOWN,
    LEFT,
    RIGHT
}

public class UserInput :IInitializable, IFixedTickable {
    public event Action<Direction> OnSwipeDetected;

    Vector2 firstPressPos;
    Vector2 secondPressPos;
    Vector2 currentSwipe;

    float touchHoldTimer;

    public void Initialize() {

    }

    public void FixedTick() {
        if(Input.GetMouseButtonDown(0)) {
            //save began touch 2d point
            firstPressPos = Input.mousePosition;
            touchHoldTimer = 0.2f;
        }
        if(Input.GetMouseButton(0)) {
            if(touchHoldTimer > 0) {
                touchHoldTimer -= Time.unscaledDeltaTime;
                if(touchHoldTimer <= 0) {
                    firstPressPos = Input.mousePosition;
                    touchHoldTimer = 0.2f;
                }
            }
            //save ended touch 2d point
            secondPressPos = Input.mousePosition;

            //create vector from the two points
            currentSwipe = secondPressPos - firstPressPos;

            //normalize the 2d vector
            currentSwipe.Normalize();

            //swipe upwards
            if(currentSwipe.y > 0.5f && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f) {
                OnSwipeDetected(Direction.UP);
            }
            //swipe down
            if(currentSwipe.y < -0.5f && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f) {
                OnSwipeDetected(Direction.DOWN);
            }
            //swipe left
            if(currentSwipe.x < -0.5f && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f) {
                OnSwipeDetected(Direction.LEFT);
            }
            //swipe right
            if(currentSwipe.x > 0.5 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f) {
                OnSwipeDetected(Direction.RIGHT);
            }
        }
    }

}
