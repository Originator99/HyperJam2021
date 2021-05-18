﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TutorialManager : MonoBehaviour {
    public GameObject tutorialOverlay;
    public GameObject highlightPanelsOverlay;
    public TutorialPopup dialogBox;
    public Button overlayClose;

    private Queue<TutorialData> tutorials;
    private bool tutorialActive;

    private DiContainer _container;

    [Inject]
    public void Construct(SignalBus signalBus, DiContainer container) {
        signalBus.Subscribe<TutorialSignal>(OnTutorialSignal);
        _container = container;
    }

    private void OnTutorialSignal(TutorialSignal signalData) {
        if(tutorials == null) {
            tutorials = new Queue<TutorialData>();
        }
        if(signalData.tutorials != null && signalData.tutorials.Count > 0) {
            foreach(var data in signalData.tutorials) {
                if(data != null) {
                    tutorials.Enqueue(data);
                }
            }
        } else {
            Debug.LogError("Tutorial signal triggered but tutorial slides are empty");
        }
    }

    private void Start() {
        EndTutorial();
    }

    private void Update() {
        if(tutorials != null && tutorials.Count > 0 && !tutorialActive) {
            ShowTutorial(tutorials.Dequeue());
        }
    }

    private void ShowTutorial(TutorialData data) {
        if(data != null) {
            tutorialOverlay.SetActive(true);
            tutorialActive = true;
            CheckAndShowDialogBox(data);
            CheckAndHighlightPanels(data.highLightPanels);
            SetDismissArea(data);
        } else {
            Debug.LogError("Cannot show tutorial, data is null");
        }
    }

    private void CheckAndShowDialogBox(TutorialData data) {
        if(data != null && !string.IsNullOrEmpty(data.header) && !string.IsNullOrEmpty(data.content)) {
            dialogBox.ShowDialogBox(data.header, data.content);
        }
    }
    private void CheckAndHighlightPanels(List<Transform> panelsToHighlight) {
        if(panelsToHighlight != null && panelsToHighlight.Count > 0) {
            foreach(var panel in panelsToHighlight) {
                //Transform clone = Instantiate(panel, highlightPanelsOverlay.transform).transform;
                Transform clone = _container.InstantiatePrefab(panel, highlightPanelsOverlay.transform).transform;
                clone.position = panel.position;
                clone.localScale = panel.lossyScale;
            }
        }
    }

    private void SetDismissArea(TutorialData data) {
        if(data != null) {
            overlayClose.onClick.RemoveAllListeners();
            overlayClose.onClick.AddListener(delegate () {
                OnDismissed(data);
            });
        } else {
            //fallback
            overlayClose.onClick.RemoveAllListeners();
            overlayClose.onClick.AddListener(delegate () {
                EndTutorial();
            });
        }
    }

    private async void OnDismissed(TutorialData data) {
        dialogBox.CloseDialogBox();
        await System.Threading.Tasks.Task.Delay(200);
        EndTutorial();
        data.callback?.Invoke();
        tutorialActive = false;
    }


    private void EndTutorial() {
        if(tutorials != null && tutorials.Count <= 0) {
            tutorialOverlay.SetActive(false);
        }

        tutorialActive = false;
        foreach(Transform clone in highlightPanelsOverlay.transform) {
            if(clone != null) {
                Destroy(clone.gameObject);
            }
        }
    }
}
