using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelManager : ITickable, IFixedTickable {
    private readonly LevelRandomizer _levelLogic;
    private readonly Settings _settings;
    private readonly Player _player;
    private readonly SignalBus _signalBus;

    private Grid2D grid;

    private Transform parentForBricks;

    private Node2D startPoint, endPoint;

    private float levelRestartTimer;

    public LevelManager(Settings settings, LevelRandomizer levelLogic, Player player, SignalBus signalBus) {
        _settings = settings;
        _levelLogic = levelLogic;
        _player = player;
        _signalBus = signalBus;
        _signalBus.Subscribe<PlayerDiedSignal>(OnPlayerDied);
        levelRestartTimer = -1;

        CreateParentForBricks();
    }

    public void Tick() {
        //Similar to update
        if(levelRestartTimer > 0) {
            levelRestartTimer -= Time.unscaledDeltaTime;
            if(levelRestartTimer <= 0) {
                ResetLevel();
                levelRestartTimer = -1;
            }
        }
    }

    public void FixedTick() {
        //similar to fixedUpdate
    }

    public void Start() {
        ResetLevel();
    }

    public void ResetLevel() {
        if(grid == null) {
            grid = new Grid2D(_settings.worldSize, _settings.gridSpace);
        }

        SetStartPoint();
        SetEndPoint();

        for(int x = 0; x < grid.gridSizeX; x++) {
            for(int y = 0; y < grid.gridSizeY; y++) {
                Node2D currentNode = grid.Grid[x, y];
                Brick brick = null;
                if(currentNode.data == null) {
                    brick = GenerateEmptyBrickCell();
                } else {
                    brick = currentNode.data as Brick;
                }

                int neighborsWithBombs = 0;
                var neighbors = GetNeighborNodes(currentNode, true);
                if(neighbors != null) {
                    neighborsWithBombs = neighbors.FindAll(t => t.obstacle == true).Count;
                }

                BrickData data = _levelLogic.GenerateRandomBrick(currentNode.ID, currentNode.worldPosition, neighborsWithBombs);
                
                if(endPoint.worldPosition == currentNode.worldPosition) {
                    data.type = BrickType.END; //forcing end brick to be end brick
                    data.renderData.brickSprite = _settings.endSprite;
                }

                if(data.type == BrickType.BOMB) {
                    currentNode.SetObstacle(true);
                } else {
                    currentNode.SetObstacle(false);
                }

                brick.InitializeBrick(data);

                currentNode.SetData(brick);

                //for hirachy to look better 
                brick.transform.name = "Brick" + x + "," + y;
            }
        }

        FindPath(startPoint, endPoint);
    }

    public Node2D GetBrickInDirection(Direction direction, Node2D currentNode, int level = 1) {
        if(currentNode != null) {
            return grid.GetNodeInDirection(direction, currentNode, level);
        } else {
            Debug.LogError("Current Node is null, cannot find the next node in direction " + direction.ToString());
            return null;
        }
    }

    public List<Node2D> GetBricksInDirection(Direction direction, Node2D currentNode, int levels) {
        if(currentNode != null) {
            List<Node2D> nodes = new List<Node2D>();
            for(int i = 1; i <= levels; i++) {
                Node2D node = GetBrickInDirection(direction, currentNode, i);
                if(node != null) {
                    nodes.Add(node);
                }
            }
            return nodes;
        } else {
            Debug.LogError("Current Node is null, cannot find the next node in direction " + direction.ToString());
            return null;
        }
    }

    public List<Node2D> GetNeighborNodes(Node2D currentNode, bool checkDiagonal) {
        return grid.GetNeighbors(currentNode, checkDiagonal);
    }


    private void SetStartPoint() {
        startPoint = grid.Grid[Random.Range(0, grid.gridSizeX / 5), Random.Range(0, grid.gridSizeY / 5)];
        _player.ResetPlayerPosition(startPoint);
    }

    private void SetEndPoint() {
        endPoint = grid.Grid[Random.Range(grid.gridSizeX / 2, grid.gridSizeX), Random.Range(grid.gridSizeY / 2, grid.gridSizeY)];
        Debug.Log("End Point : " + endPoint.ID);

    }

    private void FindPath(Node2D start, Node2D end) {
        Pathfinding2D finder = new Pathfinding2D(grid);
        finder.FindPath(start, end);

        if(grid.path != null) {
            foreach(var pathNode in grid.path) {
                if(pathNode.data is Brick) {
                    Brick brick = pathNode.data as Brick;
                    if(brick.currentType != BrickType.END)
                        brick.DestroyBrick();
                }
            }
        } else {
            Debug.LogError("Path not found for end point " + end.ID);
        }
    }

    private Brick GenerateEmptyBrickCell() {
        GameObject obj = GameObject.Instantiate(_settings.emptyBrickPrefab, parentForBricks);
        Brick controller = obj.GetComponent<Brick>();
        if(controller == null) {
            controller = obj.AddComponent<Brick>();
        }
        return controller;
    }

    private void CreateParentForBricks() {
        GameObject parentForBricks = new GameObject {
            name = "Bricks"
        };
        this.parentForBricks = parentForBricks.transform;
    }

    private void OnPlayerDied(PlayerDiedSignal playerDiedData) {
        _player.ChangeState(PlayerStates.Dead);
        levelRestartTimer = 2.5f;
    }


    [System.Serializable]
    public class Settings {
        public int difficultyID = 1;
        public float gridSpace = 2.5f;
        public Vector3 worldSize;
        public float bombChance = 0.2f;
        public GameObject emptyBrickPrefab;
        public Sprite endSprite;
        public List<BrickGraphicData> bricksData;
    }
}