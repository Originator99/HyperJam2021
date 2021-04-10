using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour {
    public LevelLogic.Settings levelSettings;

    public GameObject levelRoot;

    public GameObject emptyPrefab;
    public Transform brickGridParent;
    public Vector3 worldSize;
    public float gridSpace = 1.5f;

    private LevelLogic levelLogic;

    public void GenerateDefaultLevelGrid() {
        Grid2D grid = new Grid2D(worldSize, gridSpace);

        for(int x = 0; x < grid.gridSizeX; x++) {
            for(int y = 0; y < grid.gridSizeY; y++) {
                Node2D currentNode = grid.Grid[x, y];
                Brick brick = GenerateEmptyBrick();
                brick.transform.position = currentNode.worldPosition;

                currentNode.SetData(brick);
            }
        }

        levelLogic = new LevelLogic(grid, levelSettings);
        levelLogic.GenerateRandomPath();
    }

    public void RandomizeLevel() {
        if(levelLogic != null) {
            levelLogic.Randomize();
        } else {
            Debug.LogError("Level logic is not initialized");
        }
    }

    public void DestroyLevel() {
        if(brickGridParent != null) {
            foreach(Transform child in brickGridParent) {
                DestroyImmediate(child.gameObject);
            }
        } else {
            Debug.LogError("Brick parent is null, cannot destroy level");
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

}
