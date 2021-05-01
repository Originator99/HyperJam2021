using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class EndScreenUI : MonoBehaviour {
    public TMP_Text finalScore;
    public Button playAgain;

    GameManager _gameManager;
    ScoreHelper _scoreHelper;

    [Inject]
    public void Construct(GameManager gameManager, ScoreHelper scoreHelper) {
        _gameManager = gameManager;
        _scoreHelper = scoreHelper;
    }

    private void Start() {
        playAgain.onClick.RemoveAllListeners();
        playAgain.onClick.AddListener(delegate() {
            Hide();

        });
    }

    public void Show(bool hasWon) {
        if(!gameObject.activeSelf) {
            gameObject.SetActive(true);

            finalScore.text = "TOTAL MOVES MADE : " + _scoreHelper.GetMovesMade();
        }
    }
    public void Hide() {
        if(gameObject.activeSelf) {
            gameObject.SetActive(false);
        }
    }
}
