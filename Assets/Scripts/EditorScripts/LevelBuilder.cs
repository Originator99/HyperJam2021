using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Made for Editor, not yet optimized to run in game.
/// Handles automatic level creation which can be later edited by admin
/// </summary>
public class LevelBuilder : MonoBehaviour {
    public LevelLogic.Settings levelSettings;

    public GameObject levelRoot;

    public GameObject emptyPrefab;
    public Transform brickGridParent;
    public Vector3 worldSize;
    public float gridSpace = 1.5f;

    private LevelLogic levelLogic;
    
    private Level levelController;

    public void GenerateDefaultLevelGrid() {
        CreateLevelScript();

        Grid2D grid = new Grid2D(worldSize, gridSpace);

        for(int x = 0; x < grid.gridSizeX; x++) {
            for(int y = 0; y < grid.gridSizeY; y++) {
                Node2D currentNode = grid.Grid[x, y];
                Brick brick = GenerateEmptyBrick();
                brick.transform.position = currentNode.worldPosition;

                currentNode.SetData(brick);

                AddBrickToLevelController(brick);
            }
        }
        Debug.Log("Bricks added to level controller");

        levelLogic = new LevelLogic(grid, levelSettings);
        levelLogic.GenerateRandomPath();
        AddPathToLevelController();
    }

    public void RandomizeLevel() {
        if(levelLogic != null) {
            levelLogic.Randomize();
        } else {
            Debug.LogError("Level logic is not initialized");
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
                foreach(Brick brick in levelController.levelBricks) {
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

    private Brick GenerateEmptyBrick() {
        GameObject obj = GameObject.Instantiate(emptyPrefab, brickGridParent);
        Brick controller = obj.GetComponent<Brick>();
        if(controller == null) {
            controller = obj.AddComponent<Brick>();
        }
        return controller;
    }

    private void CreateLevelScript() {
        levelController = levelRoot.GetComponent<Level>();

        if(levelController == null) {
            levelController = levelRoot.AddComponent<Level>();
        }

        if(levelController.levelBricks == null) {
            levelController.levelBricks = new List<Brick>();
        }
        if(levelController.safePathIDs == null) {
            levelController.safePathIDs = new List<string>();
        }
    }

    private void AddBrickToLevelController(Brick brick) {
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
