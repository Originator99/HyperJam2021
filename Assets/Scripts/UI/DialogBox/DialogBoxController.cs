using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialogBoxController : MonoBehaviour {
    public TMP_Text header, content;
    public Button[] dismissAreas;
    public GameObject dialogBox, dialogBoxParent;
    
    public AudioSource audioSource;


    Queue<DialogBoxData> queuedDialogBoxes = new Queue<DialogBoxData>();

    private bool showing = false;
    private Sequence tweenSequence;

    private void Start() {
        tweenSequence = DOTween.Sequence();
    }

    public void ShowDialogBox(DialogBoxData data) {
        if(data != null) {
            queuedDialogBoxes.Enqueue(data);
        } else {
            Debug.LogError("cannot show dialog box, data is null");
        }
    }

    private void Update() {
        if(!showing && queuedDialogBoxes.Count > 0) {
            showing = true;
            MakeDialogBoxVisible(queuedDialogBoxes.Dequeue());
        }
    }

    private void MakeDialogBoxVisible(DialogBoxData data) {
        header.text = data.header;
        content.text = data.content;

        if(dismissAreas != null) {
            foreach(Button btn in dismissAreas) {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(delegate () {
                    CloseDialogBox(data.callback);
                });
            }
        }
        dialogBox.SetActive(true);
        dialogBoxParent.SetActive(true);
        if(audioSource != null) {
            audioSource.Play();
        }
        dialogBox.transform.DOScale(new Vector2(1,1), 0.15f).OnComplete(delegate () {
        });
    }

    private void CloseDialogBox(System.Action callback) {
        if(showing) {
            if(dismissAreas != null) {
                foreach(Button btn in dismissAreas) {
                    btn.onClick.RemoveAllListeners();
                }
            }
            dialogBox.transform.DOScale(Vector2.zero, 0.15f).OnComplete(delegate () {
                showing = false;
                callback?.Invoke();
                dialogBox.SetActive(false);
                dialogBoxParent.SetActive(false);
            });
        }
    }
}

public class DialogBoxData {
    public string header;
    public string content;
    public System.Action callback;
}
