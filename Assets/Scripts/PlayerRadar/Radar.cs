using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Radar :ITickable {
    private readonly SignalBus _signalBus;
    private readonly Settings _settings;
    private readonly Player _player;

    private List<Collider2D> colliders;

    private int bricksDestroyed;

    public event Action OnRadarReady;

    public Radar(SignalBus signalBus, Settings settings, Player player) {
        _settings = settings;
        _player = player;
        _signalBus = signalBus;

        _signalBus.Subscribe<BrickDestroyedSignal>(OnBrickDestroyed);

        colliders = new List<Collider2D>();
    }

    public bool CanActivateRadar() {
        return bricksDestroyed >= _settings.bricksToDestroyForRadar;
    }

    public void ActivateRadar() {
        if(bricksDestroyed >= _settings.bricksToDestroyForRadar) {
            colliders.Clear();
            bricksDestroyed = 0;
            CheckBombsAroundPlayer();
        }
    }

    public void Tick() {

    }

    private void OnBrickDestroyed(BrickDestroyedSignal signalData) {
        bricksDestroyed++;
        if(CanActivateRadar())
            OnRadarReady?.Invoke();
    }

    private void CheckBombsAroundPlayer() {
        Collider2D[] itemsInRange = Physics2D.OverlapCircleAll(_player.transform.position, _settings.radius, _settings.layersToShowOnRadar);
        if(itemsInRange != null) {
            foreach(Collider2D collider in itemsInRange) {
                if(!colliders.Contains(collider)) {
                    BaseBrick brick = collider.GetComponent<BaseBrick>();
                    if(brick != null && brick.currentType == BrickType.BOMB) {
                        GameObject.Instantiate(_settings.radarEnemyPrefab, collider.transform.position, Quaternion.identity);
                        colliders.Add(collider);
                    }
                }
            }
        }
    }

    [System.Serializable]
    public class Settings {
        public GameObject radarEnemyPrefab;
        public LayerMask layersToShowOnRadar;
        public float radius;
        public int bricksToDestroyForRadar;
    }
}
