using Zenject;
using UnityEngine;

public class PlayerStateDead :PlayerState {
    private readonly Settings _settings;
    private readonly Player _player;

    public PlayerStateDead(Settings settings, Player player) {
        _settings = settings;
        _player = player;
    }

    public override void Start() {
        if(_settings.deadEffect != null) {
            GameObject.Instantiate(_settings.deadEffect, _player.transform.position, Quaternion.identity);
        }
        _player.gameObject.SetActive(false);
    }

    public override void Update() {
    
    }


    [System.Serializable]
    public class Settings {
        public GameObject deadEffect;
    }

    public class Factory :PlaceholderFactory<PlayerStateDead> {
    }
}
