using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class StartScreenUI : MonoBehaviour {
    private GameManager _gameManager;
    [Inject]
    public void Construct(GameManager gameManager) {
        _gameManager = gameManager;
    }

    public Button startButton;

    private void Start() {
        startButton.onClick.RemoveAllListeners();
        startButton.onClick.AddListener(delegate() {
            HideStartScreen();
            _gameManager.BuildLevel();
        });
    }

    public void ShowStartScreen() {
        if(!gameObject.activeSelf) {
            gameObject.SetActive(true);
        }
    }
    public void HideStartScreen() {
        if(gameObject.activeSelf) {
            //will add animation later
            gameObject.SetActive(false);
        }
    }
}
