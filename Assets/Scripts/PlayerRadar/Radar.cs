using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            //CheckBombsAroundPlayer();
            //CheckAndReactivateRadar();
            ShowRipple();
        }
    }

    public int GetTotalRadars() {
        return radarsAvailable;
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

    private async void ShowRipple() {
        if(_settings.radarRipple != null) {
            GameObject rippleGO = GameObject.Instantiate(_settings.radarRipple, _player.transform.position, Quaternion.identity);
            ParticleSystem ripple = rippleGO.GetComponent<ParticleSystem>();
            if(ripple != null){
                ParticleSystem.MainModule main = ripple.main;
                main.startSize = _settings.radius * _settings.radius / 2;

                await Task.Delay(500);

                CheckBombsAroundPlayer();
                CheckAndReactivateRadar();
            }
        }
    }

    [System.Serializable]
    public class Settings {
        public GameObject radarEnemyPrefab;
        public GameObject radarRipple;
        public LayerMask layersToShowOnRadar;
        public float radius;
    }
}
