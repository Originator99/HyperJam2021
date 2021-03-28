using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreHelper {

    private int movesMade;
    public ScoreHelper() {
        movesMade = 0;
    }

    public void ResetScore() {
        movesMade = 0;
    }

    public void UpdateMoves(int amount) {
        movesMade += amount;
    }
    public int GetMovesMade() {
        return movesMade;
    }
}
