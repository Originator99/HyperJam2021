﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class Pattern : MonoBehaviour {
    public Transform playerIcon;

    public TMP_Text north, south, east, west;
    private Settings _settings;
    private Player _player;
    private LevelManager _levelManager;

    private bool scanned;
    private Vector3 lastPlayerPosition;

    [Inject]
    public void Construct(Settings settings, Player player, LevelManager levelManager) {
        _settings = settings;
        _player = player;
        _levelManager = levelManager;
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
    public void Show() {
        gameObject.SetActive(true);
    }

    private void Update() {
        playerIcon.rotation = _player.transform.rotation;

        if(lastPlayerPosition != _player.transform.position) {
            lastPlayerPosition = _player.transform.position;
            scanned = false;
        }

        if(!scanned) {
            ScanForBombs();
        }
    }

    private void ScanForBombs() {
        var northBricks = _levelManager.GetBricksInDirection(Direction.UP, _player.currentBrickCell, _settings.radius);
        CheckAndRenderRadar(northBricks, north);

        var southBricks = _levelManager.GetBricksInDirection(Direction.DOWN, _player.currentBrickCell, _settings.radius);
        CheckAndRenderRadar(southBricks, south);

        var westBricks = _levelManager.GetBricksInDirection(Direction.LEFT, _player.currentBrickCell, _settings.radius);
        CheckAndRenderRadar(westBricks, west);

        var eastBricks = _levelManager.GetBricksInDirection(Direction.RIGHT, _player.currentBrickCell, _settings.radius);
        CheckAndRenderRadar(eastBricks, east);
        
        scanned = true;
    }

    private void CheckAndRenderRadar(List<Node2D> bricks, TMP_Text textHolder) {
        if(bricks != null) {
            int distance = 0;
            int bombFoundAt = 1;
            foreach(var brick in bricks) {
                if(brick.data is Brick) {
                    Brick controller = brick.data as Brick;
                    if(controller != null) {
                        distance++;

                        if(controller.currentType == BrickType.BOMB) {
                            bombFoundAt = distance;
                            break;
                        }
                    }
                }
            }

            int probability = Mathf.RoundToInt(GetProbabilityOfNextBrickBomb(bombFoundAt) + GetProbabilityOffset());
            if(probability <= 0)
                probability = 1;
            else if(probability > 80)
                probability = Random.Range(75, 99);

            if(probability > 5f) {
                textHolder.text = probability.ToString() + "%";
            } else {
                textHolder.text = "";
            }
        }
    }

    public float GetProbabilityOfNextBrickBomb(int brickPositionFromPlayer) {
        if(brickPositionFromPlayer == 0) {
            Debug.LogWarning("Brick with bomb found at 0th position, changing it to 1");
            brickPositionFromPlayer = 1;
        }
        return (float)(100 / brickPositionFromPlayer);
    }

    public float GetProbabilityOffset() {
        return Random.Range(-_settings.radarOffset, _settings.radarOffset);
    }

    [System.Serializable]
    public class Settings {
        public int radius;
        public int radarOffset;
    }
}
