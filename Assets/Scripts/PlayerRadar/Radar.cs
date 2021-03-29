using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Radar :ITickable {
    private readonly Settings _settings;
    private readonly Player _player;

    private List<Collider2D> colliders;

    private float timer;

    public Radar(Settings settings, Player player) {
        _settings = settings;
        _player = player;

        colliders = new List<Collider2D>();
    }

    public void Tick() {
        if(timer <= 0) {
            colliders.Clear();
            timer = _settings.radarShowTimer;
        }
        else if(timer > 0) {
            timer -= Time.deltaTime;
        }

        Collider2D[] itemsInRange = Physics2D.OverlapCircleAll(_player.transform.position, _settings.radius, _settings.layersToShowOnRadar);
        if(itemsInRange  != null) {
            foreach(Collider2D collider in itemsInRange) {
                if(!colliders.Contains(collider)) {
                    Brick brick = collider.GetComponent<Brick>();
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
        public float radarShowTimer;
    }
}
