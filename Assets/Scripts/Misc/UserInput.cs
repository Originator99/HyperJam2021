using UnityEngine;
public enum Direction {
    NONE,
    UP,
    DOWN,
    LEFT,
    RIGHT
}

public class UserInput {

    public static Vector2 ConvertDirection(Direction direction) {
        switch(direction) {
            case Direction.DOWN:
                return Vector2.down;
            case Direction.UP:
                return Vector2.up;
            case Direction.LEFT:
                return Vector2.left;
            case Direction.RIGHT:
                return Vector2.right;
            default:
                return Vector2.zero;
        }
    }
}