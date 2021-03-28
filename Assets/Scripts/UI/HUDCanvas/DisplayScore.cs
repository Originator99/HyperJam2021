using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class DisplayScore :MonoBehaviour {
    private ScoreHelper _scoreHelper;
    public TMP_Text score;

    private int currentScore = 0;

    [Inject]
    public void Construct(ScoreHelper scoreHelper) {
        _scoreHelper = scoreHelper;
    }

    private void Update() {
        UpdateScore(_scoreHelper.GetMovesMade());
    }

    public void Show(){
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    private void UpdateScore(int amount) {
        if(currentScore != amount) {
            currentScore = amount;
            score.text = currentScore.ToString();
        }
    }
}
