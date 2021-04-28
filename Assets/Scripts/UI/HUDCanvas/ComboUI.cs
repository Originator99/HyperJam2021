using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using Zenject;

public class ComboUI : MonoBehaviour {
    public TMP_Text comboAmount, comboText;

    private SignalBus _signalBus;
    private Settings _settings;

    private float comboTimer;
    private int currentCombo;
    private Vector3 originalPosition;

    [Inject]
    public void Construct(SignalBus signalBus, Settings settings) {
        _settings = settings;
        _signalBus = signalBus;

        _signalBus.Subscribe<BrickDestroyedSignal>(OnBrickDestroyed);
    }

    private void Start() {
        originalPosition = transform.position;

        HideCombo();
    }

    private void Update() {
        if(comboTimer > 0) {
            comboTimer -= Time.deltaTime;
            if(comboTimer <= 0) {
                ResetCombo();
            }
        }
    }

    private void OnBrickDestroyed(BrickDestroyedSignal signalData) {
        currentCombo++;
        if(comboTimer > 0) {
            if(currentCombo > _settings.minCombo) {
                ShowCombo();
            }
        }
        comboTimer = _settings.comboTimeout;
    }

    private void ShowCombo() {
        comboAmount.enabled = true;
        comboAmount.text = "<size=35>x</size>" + currentCombo;
        comboText.enabled = true;

        comboAmount.transform.DOShakePosition(2.5f, new Vector3(10, 0, 0)).OnComplete(delegate() {
            transform.position = originalPosition;
        });
    }

    private void ResetCombo() {
        currentCombo = 0;
        comboTimer = 0;
        HideCombo();
    }

    private void HideCombo() {
        comboAmount.enabled = false;
        comboText.enabled = false;
    }

    [System.Serializable]
    public class Settings {
        public int minCombo = 2;
        public float comboTimeout;
    }
}
