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

    private int radarsAvailable;

    public event Action OnRadarReady, OnRadarNotReady;

    public Radar(SignalBus signalBus, Settings settings, Player player) {
        _settings = settings;
        _player = player;
        _signalBus = signalBus;

        _signalBus.Subscribe<LevelStartedSignal>(OnLevelStarted);

        colliders = new List<Collider2D>();
    }

    public bool CanActivateRadar() {
        return radarsAvailable > 0;
    }

    public void ActivateRadar() {
        if(CanActivateRadar()) {
            colliders.Clear();
            radarsAvailable--;
            CheckBombsAroundPlayer();
            CheckAndReactivateRadar();
        }
    }

    public void Tick() {

    }

    private void OnLevelStarted(LevelStartedSignal signalData) {
        radarsAvailable = signalData.levelSettings.totalRadars;
        CheckAndReactivateRadar();
    }

    private void CheckAndReactivateRadar() {
        if(CanActivateRadar())
            OnRadarReady?.Invoke();
        else
            OnRadarNotReady?.Invoke();
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
    }
}
