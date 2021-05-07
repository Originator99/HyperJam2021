using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour {
    public CanvasGroup canvasGroup;
    public Image fillImage;

    public void ShowLoadingScreen(float forTime) {
        fillImage.fillAmount = 0;
        canvasGroup.alpha = 0;
        gameObject.SetActive(true);
        canvasGroup.DOFade(1, 0.5f).OnComplete(delegate () {
            fillImage.DOFillAmount(1, forTime).OnComplete(delegate () {
                HideLoadingScreen();
            });
        });
    }

    private void HideLoadingScreen() {
        canvasGroup.DOFade(0, 0.5f).OnComplete(delegate () {
            gameObject.SetActive(false);
        });
    }
}
