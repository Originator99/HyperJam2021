using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopBox : MonoBehaviour, IMenuItem {
    public CanvasGroup mainPanel;
    public void Hide(System.Action callback) {
        mainPanel.DOFade(1, 0.25f).OnComplete(delegate () {
            mainPanel.gameObject.SetActive(false);
            callback?.Invoke();
        });
    }

    public void Show() {
        mainPanel.gameObject.SetActive(true);
        mainPanel.DOFade(1, 0.25f).OnComplete(delegate () {

        });
    }
}
