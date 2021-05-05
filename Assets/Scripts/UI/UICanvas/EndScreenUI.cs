using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using Zenject;

public class EndScreenUI : MonoBehaviour {
    public Animator animator;
    public CanvasGroup canvasGroup;
    public TMP_Text bricksDestroyed, header;

    public Color wonHeaderColor, lostHeaderColor;

    private ScoreHelper _scoreHelper;
    private GameManager _gameManager;

    [Inject]
    public void Construct(GameManager gameManager, ScoreHelper scoreHelper) {
        _gameManager = gameManager;
        _scoreHelper = scoreHelper;
    }

    private void OnEnable() {
        canvasGroup.alpha = 1;
        animator.enabled = true;
        animator.Play("Complete");
    }

    public async void AutoDisable() {
        await System.Threading.Tasks.Task.Delay(2000);
        Hide();
    }

    public void Show(bool hasWon) {
        if(!gameObject.activeSelf) {
            bricksDestroyed.text = _scoreHelper.GetMovesMade().ToString();
            header.text = hasWon ? "LEVEL COMPLETE" : "LEVEL LOST";
            header.color = hasWon ? wonHeaderColor : lostHeaderColor;

            gameObject.SetActive(true);

        }
    }
    public void Hide() {
        if(gameObject.activeSelf) {
            animator.enabled = false;
            canvasGroup.DOFade(0, 0.5f).OnComplete(delegate () {
                gameObject.SetActive(false);
                _gameManager.ShowStartScreen();
            });
        }
    }
}
