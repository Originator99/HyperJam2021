using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class Level : MonoBehaviour {
    public Settings levelSettings;
    public List<BaseBrick> levelBricks;
    public List<string> safePathIDs;

    public LayerMask brickLayerMask;

    public void TogglePath(bool state) {
        if(levelBricks != null && safePathIDs != null) {
            for(int i = 0; i < safePathIDs.Count; i++) {
                int index = levelBricks.FindIndex(x => x.ID == safePathIDs[i]);
                if(index >= 0) {
                    if(state) {
                        levelBricks[index].SwitchToPath();
                    } else {
                        levelBricks[index].SwitchToOriginalState();
                    }
                }
            }
        }
    }

    /// <summary>
    /// Shuffles the current level items
    /// </summary>
    public async void ShuffleLevel() {
        if(levelBricks != null) {
            //making a copy to shuffle items
            List<BaseBrick> levelCopy = new List<BaseBrick>(levelBricks);
            //levelCopy.RemoveAll(x => safePathIDs.Any(y => y == x.IDOnGrid));
            List<BaseBrick> shuffle = new List<BaseBrick>();
            while(levelCopy.Count != 0) {
                int index = Random.Range(0, levelCopy.Count);
                shuffle.Add(levelCopy[index]);
                levelCopy.RemoveAt(index);
            } //shuffling the bricks

            shuffle.RemoveAll(x => safePathIDs.Any(y => y == x.ID));

            int shuffleAnimIndex = 0;
            //reassigning the shuffled bricks to thier positions
            foreach(BaseBrick brick in levelBricks) {
                if(shuffleAnimIndex % 5 == 0) {
                    await Task.Delay(50);
                }

                brick.DoShuffleHint();

                if(!safePathIDs.Contains(brick.ID)) {
                    int sIndex = levelBricks.FindIndex(x => x.ID == shuffle[0].ID);
                    Vector2 tempPos = levelBricks[sIndex].transform.position;
                    levelBricks[sIndex].SwitchPositions(brick.transform.position);
                    brick.SwitchPositions(tempPos);
                }
                brick.ResetBrick();
                shuffleAnimIndex++;
            }
        }
        //resetting start brick
        BaseBrick b = GetStartBrick();
        if(b != null) {
            b.ResetBrick();
        }
    }

    public BaseBrick GetStartBrick() {
        if(levelBricks != null && safePathIDs != null) {
            if(safePathIDs.Count > 0) {
                int index = levelBricks.FindIndex(x => x.ID == safePathIDs[0]);
                if(index >= 0) {
                    return levelBricks[index];
                }
            }
        }
        return null;
    }

    public BaseBrick GetBrickInDirectionFrom(BaseBrick currentBrick, Direction direction, Vector2 position) {
        RaycastHit2D[] hits = Physics2D.RaycastAll(position, UserInput.ConvertDirection(direction), 5f, brickLayerMask); //layerMask of Brick is 9
        foreach(RaycastHit2D hit in hits) {
            if(hit.collider != null) {
                BaseBrick brick = hit.collider.GetComponent<BaseBrick>();
                if(brick != null && currentBrick.ID != brick.ID) {
                    return brick;
                }
            }
        }
        return null;
    }

    public List<BaseBrick> GetPortalBricks() {
        return levelBricks.FindAll(x => x.currentType == BrickType.END);
    }

    [System.Serializable]
    public class Settings {
        public int chapterID;
        public int levelID;
        public Vector3 worldSize;
        public float gridSpace;
        public float bombChance;
        public int totalRadars;
    }
}
