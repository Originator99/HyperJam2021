using System;
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

    private Settings _settings;
    private DiContainer _container;

    private List<HighlightedPanels> currentHighlightedPanels;

    [Inject]
    public void Construct(SignalBus signalBus, DiContainer container, Settings settings) {
        signalBus.Subscribe<TutorialSignal>(OnTutorialSignal);
        _container = container;
        _settings = settings;

        currentHighlightedPanels = new List<HighlightedPanels>();
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
                //Transform clone = _container.InstantiatePrefab(panel, highlightPanelsOverlay.transform).transform;
                //clone.position = panel.position;
                //clone.localScale = panel.lossyScale;

                HighlightedPanels highlight = new HighlightedPanels() {
                    panel = panel,
                    originalLayerMask = panel.gameObject.layer,
                    originalSortingMask = panel.GetComponent<SpriteRenderer>() ? panel.GetComponent<SpriteRenderer>().sortingLayerID : -1
                };

                SpriteRenderer spriteRender = panel.GetComponent<SpriteRenderer>();
                if(spriteRender != null) {
                    spriteRender.sortingLayerID = SortingLayer.NameToID(_settings.TutorialSortingLayer);
                }
                panel.gameObject.layer = LayerMask.NameToLayer(_settings.TutorialLayerMask);

                currentHighlightedPanels.Add(highlight);
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

        ResetHighlightedPanelLayers();
    }

    private void ResetHighlightedPanelLayers() {
        if(currentHighlightedPanels != null && currentHighlightedPanels.Count > 0) {
            foreach(var data in currentHighlightedPanels) {
                if(data.panel != null) {
                    SpriteRenderer spriteRender = data.panel.GetComponent<SpriteRenderer>();
                    if(spriteRender != null) {
                        spriteRender.sortingLayerID = data.originalSortingMask;
                    }
                    data.panel.gameObject.layer = data.originalLayerMask;
                }
            }
            currentHighlightedPanels.Clear();
        }
    }

    [System.Serializable]
    public class Settings {
        public string TutorialSortingLayer;
        public string TutorialLayerMask;
    }

    private class HighlightedPanels {
        public Transform panel;
        public int originalLayerMask;
        public int originalSortingMask;
    }
}
