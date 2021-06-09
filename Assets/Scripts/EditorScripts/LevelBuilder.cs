using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Made for Editor, not yet optimized to run in game.
/// Handles automatic level creation which can be later edited by admin
/// </summary>

public class LevelBuilder : MonoBehaviour {
    [Header("Level Load")]
    public GameObject loadedLevelGO;

    [Header("Level Settings")]
    public int chapterID;
    public int levelID;
    public float bombChance;
    public Vector3 worldSize;
    public float gridSpace = 1.5f;

    [Space(5)]
    public GameGraphics gameGraphics;

    [Space(5)]
    public GameObject levelRoot;

    public Transform brickGridParent;

    [Space(10)]
    public List<BaseBrick> switchBricks;
    [Space(10)]
    public List<BaseBrick> safePath;

    private LevelLogic levelLogic;
    
    private Level levelController;

    public void GenerateDefaultLevelGrid() {

        CreateLevelScript();

        Grid2D grid = new Grid2D(worldSize, gridSpace);
        levelLogic = new LevelLogic(grid, gameGraphics);

        for(int x = 0; x < grid.gridSizeX; x++) {
            for(int y = 0; y < grid.gridSizeY; y++) {
                Node2D currentNode = grid.Grid[x, y];
                BaseBrick brick = GenerateRandomBaseBrick(currentNode.ID);
                if(brick != null) {
                    brick.transform.SetParent(brickGridParent);
                    brick.SwitchPositions(currentNode.worldPosition);

                    AddBrickToLevelController(brick);
                }
            }
        }
        Debug.Log("Bricks added to level controller");
        AddPathToLevelController();
    }

    public void LoadLevel() {
        if(loadedLevelGO != null) {
            GameObject levelPrefab = Instantiate(loadedLevelGO, levelRoot.transform.parent);

            var controller = levelPrefab.GetComponent<Level>();

            levelController = controller;
            levelRoot = controller.gameObject;
            levelLogic = new LevelLogic(null, gameGraphics);

            chapterID = levelController.levelSettings.chapterID;
            levelID = levelController.levelSettings.levelID;
            bombChance = levelController.levelSettings.bombChance;
            worldSize = levelController.levelSettings.worldSize;
            gridSpace = levelController.levelSettings.gridSpace;
        } else {
            Debug.LogError("Cannot load level, level is null");
        }
    }

    public void ShuffleLevel() {
        if(levelController != null) {
            levelController.ShuffleLevel();
        }
        else{
            Debug.LogError("Level controller is null");
        }
    }

    public void DestroyLevel() {
        if(levelController != null) {
            if(levelController.levelBricks != null) {
                foreach(BaseBrick brick in levelController.levelBricks) {
                    DestroyImmediate(brick.gameObject);
                }
                levelController.levelBricks.Clear();
                
                if(levelController.safePathIDs != null) {
                    levelController.safePathIDs.Clear();
                }
            } else {
                Debug.LogError("Level bricks are null");
            }
        } else {
            Debug.LogError("levelController is null, cannot destroy level");
        }
        if(levelLogic != null) {
            levelLogic = null;
        } else {
            Debug.LogError("Level Logic is null, have you generated a level?");
        }
    }

    public void ToggleSafePath(bool state) {
        if(levelController != null) {
            levelController.TogglePath(state);
        }
    }

    public void SaveLevelSettings() {
        levelController.levelSettings = new Level.Settings {
            chapterID = chapterID,
            levelID = levelID,
            worldSize = worldSize,
            gridSpace = gridSpace,
            bombChance = bombChance
        };
    }

    public void SwitchBrickToType(BrickType type) {
        if(switchBricks != null) {
            foreach(var brick in switchBricks) {
                SwitchBrickToType(brick, type);
            }
        } else {
            Debug.LogError("Brick that is to be switched is null");
        }
    }

    public void GenerateSafePath() {
        if(levelController != null) {
            if(safePath != null && safePath.Count > 0) {
                foreach(var brick in safePath) {
                    if(!levelController.safePathIDs.Contains(brick.ID)) {
                        if(brick.currentType == BrickType.BOMB) {
                            Debug.Log("Bomb found in way of safe path, switching to normal");
                            SwitchBrickToType(brick, BrickType.NORMAL);
                        }
                        levelController.safePathIDs.Add(brick.ID);
                    } else {
                        Debug.LogWarning("Safe path ID " + brick.ID + " is already added");
                    }
                }
            } else {
                Debug.LogError("Safe path is empty");
            }
            safePath.Clear();
        }
    }

    private void SwitchBrickToType(BaseBrick brick, BrickType type) {
        GameObject obj = levelLogic.GetBrickBasedOnType(type);

        BrickData newData = new BrickData {
            gridNodeID = brick.GetComponent<BaseBrick>().ID,
            type = type
        };
        BaseBrick newB = obj.GetComponent<BaseBrick>();
        newB.Initialize(newData);
        newB.SwitchPositions(brick.transform.position);
        obj.transform.SetParent(brickGridParent);
        ReplaceInLevel(brick, newB);

        if(type == BrickType.PATH) {
            brick.SwitchToPath();
        }
    }

    private void ReplaceInLevel(BaseBrick oldB, BaseBrick newB){
        if(levelController != null && levelController.levelBricks != null) {
            int index = levelController.levelBricks.FindIndex(x => x.ID == oldB.ID);
            if(index >= 0) {
                GameObject aboutToBeDeleted = levelController.levelBricks[index].gameObject;
                levelController.levelBricks[index] = newB;
                Debug.Log("Changed original : " + oldB.currentType.ToString() + " to : " + newB.currentType.ToString());
                DestroyImmediate(aboutToBeDeleted);
            } else {
                Debug.LogError("Could not find brick in level");
            }
        }
    }

    private BaseBrick GenerateRandomBaseBrick(string id) {
        BrickType type = levelLogic.GetRandomBrickType(bombChance);
        GameObject obj = levelLogic.GetBrickBasedOnType(type);
        if(obj != null) {
            BrickData data = new BrickData {
                gridNodeID = id,
                type = type,
            };
            BaseBrick baseBrick = obj.GetComponent<BaseBrick>();
            if(baseBrick != null) {
                baseBrick.Initialize(data);
                return baseBrick;
            } else {
                Debug.LogError("Base Class not found in the brick prefab, is it being inherited from base brick?");
            }
        }
        return null;
    }

    private void CreateLevelScript() {
        levelController = levelRoot.GetComponent<Level>();

        if(levelController == null) {
            levelController = levelRoot.AddComponent<Level>();
        }

        if(levelController.levelBricks == null) {
            levelController.levelBricks = new List<BaseBrick>();
        }
        if(levelController.safePathIDs == null) {
            levelController.safePathIDs = new List<string>();
        }
    }

    private void AddBrickToLevelController(BaseBrick brick) {
        if(brick != null && levelController != null) {
            if(levelController.levelBricks != null) {
                levelController.levelBricks.Add(brick);
            }
        }
    }

    private void AddPathToLevelController() {
        if(levelController != null && levelLogic != null) {
            if(levelController.safePathIDs == null) {
                levelController.safePathIDs = new List<string>();
            }
            if(levelLogic._grid.path != null) {
                foreach(var item in levelLogic._grid.path) {
                    levelController.safePathIDs.Add(item.ID);
                }
            }
            Debug.Log("Path added to level controller");
        }
    }
}
