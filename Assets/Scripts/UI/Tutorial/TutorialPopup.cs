using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class TutorialPopup : MonoBehaviour {
    public RectTransform box;
    public TMP_Text header, content;
    public AudioSource audioSource;

    public void ShowDialogBox(string header, string content) {
        gameObject.SetActive(true);
        this.header.text = header.ToUpper();
        this.content.text = content;
        LayoutRebuilder.ForceRebuildLayoutImmediate(box);

        DialogOpenAnimation();
    }

    public void CloseDialogBox() {
        DialogCloseAnimation();
    }

    private void DialogOpenAnimation() {
        box.localScale = Vector3.zero;
        box.DOScale(Vector3.one, 0.25f).SetEase(Ease.InOutElastic);
    }

    private void DialogCloseAnimation() {
        box.DOScale(Vector3.zero, 0.15f).OnComplete(delegate () {
            gameObject.SetActive(false);
        });
    }
}
