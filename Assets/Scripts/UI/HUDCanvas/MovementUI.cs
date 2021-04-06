using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MovementUI : MonoBehaviour {
    public Button dashTrigger;

    private SignalBus _signalBus;
    private UserInput _userInput;

    private Sequence tweenSequence;
    private float defaultAlpha = 0.35f;

    [Inject]
    public void Construct(SignalBus signalBus, UserInput userInput) {
        _signalBus = signalBus;
        _userInput = userInput;

        _userInput.OnSwipeDetected += HandleSwipe;
    }

    private void HandleSwipe(Direction direction) {
        //Move(direction);
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
