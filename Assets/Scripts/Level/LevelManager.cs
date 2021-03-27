using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelManager : ITickable, IFixedTickable {
    private readonly List<Brick> levelBricks = new List<Brick>();
    private readonly LevelRandomizer _levelLogic;
    private readonly Settings _settings;

    private Transform parentForBricks;

    public LevelManager(Settings settings, LevelRandomizer levelLogic) {
        _settings = settings;
        _levelLogic = levelLogic;
        CreateParentForBricks();
        Start();
    }

    private void CreateParentForBricks() {
        GameObject parentForBricks = new GameObject {
            name = "Bricks"
        };
        this.parentForBricks = parentForBricks.transform;
    }

    public void Start() {
        levelBricks.Clear();
        Grid grid = new Grid(_settings.rows, _settings.columns, _settings.gridSpace);
        foreach(var cell in grid.gridCells) {
            Brick brick = GenerateEmptyBrickCell();
            brick.InitializeBrick(_levelLogic.GenerateRandomBrick(cell.position));
            levelBricks.Add(brick);

            //for hirachy to look better 
            brick.transform.name = "Brick" + cell.position;
        }
    }

    public void Tick() {
        //Similar to update
    }

    public void FixedTick() {
        //similar to fixedUpdate
    }

    private Brick GenerateEmptyBrickCell() {
        GameObject obj = GameObject.Instantiate(_settings.emptyBrickPrefab, parentForBricks);
        Brick controller = obj.GetComponent<Brick>();
        if(controller == null) {
            controller = obj.AddComponent<Brick>();
        }
        return controller;
    }

    [System.Serializable]
    public class Settings {
        public int difficultyID = 1;
        public float gridSpace = 2.5f;
        public int rows = 10;
        public int columns = 10;
        public float bombChance = 0.2f;
        public List<BrickGraphicData> bricksData;
        public GameObject emptyBrickPrefab;
    }
}