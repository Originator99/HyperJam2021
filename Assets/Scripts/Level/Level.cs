using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level : MonoBehaviour {
    public Settings levelSettings;
    public List<Brick> levelBricks;
    public List<string> safePathIDs;

    public LayerMask brickLayerMask;

    public void TogglePath(bool state) {
        if(levelBricks != null && safePathIDs != null) {
            for(int i = 0; i < safePathIDs.Count; i++) {
                int index = levelBricks.FindIndex(x => x.IDOnGrid == safePathIDs[i]);
                if(index >= 0) {
                    if(state) {
                        levelBricks[index].ChangeBrickState(BrickType.SAFE_PATH);
                    } else {
                        levelBricks[index].ChangeBrickState(BrickType.NORMAL);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Shuffles the current level items
    /// </summary>
    public void ShuffleLevel() {
        if(levelBricks != null) {
            //making a copy to shuffle items
            List<Brick> levelCopy = new List<Brick>(levelBricks);
            //levelCopy.RemoveAll(x => safePathIDs.Any(y => y == x.IDOnGrid));
            List<Brick> shuffle = new List<Brick>();
            while(levelCopy.Count != 0) {
                int index = Random.Range(0, levelCopy.Count);
                shuffle.Add(levelCopy[index]);
                levelCopy.RemoveAt(index);
            } //shuffling the bricks

            shuffle.RemoveAll(x => safePathIDs.Any(y => y == x.IDOnGrid));

            //reassigning the shuffled bricks to thier positions
            foreach(Brick brick in levelBricks) {
                if(!safePathIDs.Contains(brick.IDOnGrid)) {
                    int sIndex = levelBricks.FindIndex(x => x.IDOnGrid == shuffle[0].IDOnGrid);
                    Vector2 tempPos = levelBricks[sIndex].data.worldPosition;
                    levelBricks[sIndex].ChangePosition(brick.data.worldPosition);
                    brick.ChangePosition(tempPos);
                }
                brick.InitializeBrick(brick.data);
            }
        }
        //resetting start brick
        Brick b = GetStartBrick();
        if(b != null) {
            b.InitializeBrick(b.data);
        }
    }

    public Brick GetStartBrick() {
        if(levelBricks != null && safePathIDs != null) {
            if(safePathIDs.Count > 0) {
                int index = levelBricks.FindIndex(x => x.IDOnGrid == safePathIDs[0]);
                if(index >= 0) {
                    return levelBricks[index];
                }
            }
        }
        return null;
    }

    public Brick GetBrickInDirection(Direction direction, Vector2 currentPos, Brick currentBrick) {
        RaycastHit2D[] hits = Physics2D.RaycastAll(currentPos, UserInput.ConvertDirection(direction), 2f, brickLayerMask); //layerMask of Brick is 9
        foreach(var hit in hits) {
            if(hit.collider != null) {
                Brick brick = hit.collider.GetComponent<Brick>();
                if(brick != null && brick.IDOnGrid != currentBrick.IDOnGrid) {
                    return brick;
                }
            }
        }
        Debug.LogError("Could not find brick in the direction : " + direction.ToString());
        return null;
    }

    [System.Serializable]
    public class Settings {
        public Vector3 worldSize;
        public float gridSpace;
    }
}
