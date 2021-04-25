using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Immortality_PU :IPowerUp, ITickable {
    
    private readonly SignalBus _signalBus;
    private readonly Settings _settings;

    private bool isActive;
    private float timer, comboCooldownTimer;
    private int bricksDestroyed;

    public Immortality_PU(SignalBus signalBus, Settings settings) {
        _signalBus = signalBus;
        _settings = settings;

        _signalBus.Subscribe<BrickDestroyedSignal>(OnBrickDestroyed);

        Reset();
    }

    private void OnBrickDestroyed(BrickDestroyedSignal signalData) {
        if(!isActive) {
            if(comboCooldownTimer >= 0) {
                bricksDestroyed++;
                if(bricksDestroyed >= _settings.bricksToDetroy) {
                    Activate();
                }
            } else {
                Debug.Log("Starting to check for immortality");
                Reset();
                comboCooldownTimer = _settings.cooldownCheck;
            }
        }
    }

    public void Activate() {
        Debug.Log("Activating immortality");
        _signalBus.Fire(new PowerUpActivated { type = PowerUpType.IMMORTALITY });
        timer = _settings.immortality_time;
        isActive = true;
    }

    public void Reset() {
        Debug.Log("Resetting immortality");
        bricksDestroyed = 0;
        comboCooldownTimer = 0;
        isActive = false;
    }

    public void Tick() {
        if(isActive) {
            timer -= Time.deltaTime;
            if(timer <= 0) {
                isActive = false;
                _signalBus.Fire(new PowerUpDeactivated { type = PowerUpType.IMMORTALITY });
                Reset();
            }
        }
    }

    [System.Serializable]
    public class Settings {
        public float bricksToDetroy = 5f;
        public float immortality_time = 5f;
        public float cooldownCheck = 0.5f;
    }
}
