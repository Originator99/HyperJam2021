using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelManager : ITickable, IFixedTickable {
    private readonly List<Brick> levelBricks = new List<Brick>();
    private readonly Settings _settings;

    private Transform parentForBricks;

    public LevelManager(Settings settings) {
        _settings = settings;
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
            GameObject obj = new GameObject();
            obj.transform.SetParent(parentForBricks);
            Brick brick = obj.AddComponent<Brick>();
            brick.PlaceBrick(cell.position);
            levelBricks.Add(brick);
        }
    }

    public void Tick() {
        //Similar to update
    }

    public void FixedTick() {
        //similar to fixedUpdate
    }

    [System.Serializable]
    public class Settings {
        public float gridSpace = 1;
        public int rows = 1;
        public int columns = 1;
    }
}
