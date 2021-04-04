using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MovementUI : MonoBehaviour {
    public Image leftImg, rightImg, upImg, downImg;
    public Button leftTrigger, rightTrigger, upTrigger, downTrigger;
    public Button dashTrigger;

    private SignalBus _signalBus;

    private Sequence tweenSequence;
    private float defaultAlpha = 0.35f;

    [Inject]
    public void Construct(SignalBus signalBus) {
        _signalBus = signalBus;
    }

    private void Start() {
        SetButtonClicks();
    }

    private void Update() {
#if UNITY_EDITOR
        CheckInputForEditor();
#endif
    }

    private void CheckInputForEditor() {
        if(Input.GetKeyDown(KeyCode.A)) {
            leftTrigger.onClick.Invoke();
        }
        if(Input.GetKeyDown(KeyCode.D)) {
            rightTrigger.onClick.Invoke();
        }
        if(Input.GetKeyDown(KeyCode.W)) {
            upTrigger.onClick.Invoke();
        }
        if(Input.GetKeyDown(KeyCode.S)) {
            downTrigger.onClick.Invoke();
        }
        if(Input.GetKeyDown(KeyCode.Space)) {
            dashTrigger.onClick.Invoke();
        }
    }

    private void SetButtonClicks() {
        leftTrigger.onClick.RemoveAllListeners();
        leftTrigger.onClick.AddListener(delegate () {
            DoButtonClickAnimation(leftImg, 0.25f, defaultAlpha);
            Move(Direction.LEFT);
        });
        rightTrigger.onClick.RemoveAllListeners();
        rightTrigger.onClick.AddListener(delegate () {
            DoButtonClickAnimation(rightImg, 0.25f, defaultAlpha);
            Move(Direction.RIGHT);
        });
        upTrigger.onClick.RemoveAllListeners();
        upTrigger.onClick.AddListener(delegate () {
            DoButtonClickAnimation(upImg, 0.25f, defaultAlpha);
            Move(Direction.UP);
        });
        downTrigger.onClick.RemoveAllListeners();
        downTrigger.onClick.AddListener(delegate () {
            DoButtonClickAnimation(downImg, 0.25f, defaultAlpha);
            Move(Direction.DOWN);
        });
        dashTrigger.onClick.RemoveAllListeners();
        dashTrigger.onClick.AddListener(delegate() {
            Dash();
        });
    }

    private void Move(Direction direction) {
        _signalBus.Fire(new PlayerInputSignal { moveDirection = direction, doDash = false });
    }

    private void Dash() {
        _signalBus.Fire(new PlayerInputSignal { doDash = true, moveDirection = Direction.NONE });
    }

    private void DoButtonClickAnimation(Image img, float fadeOutTime, float endValue) {
        img.DOFade(1f, fadeOutTime).OnComplete(delegate () {
            img.color = new Color(img.color.r, img.color.g, img.color.b, endValue);
        });
    }
}
