﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UIManager : MonoBehaviour {
    public StartScreenUI startScreen;
    public EndScreenUI endScreen;
    public HUDCanvas hudCanvas;
    public DialogBoxController dialogBoxController;

    public void ShowStartScren() {
        hudCanvas.HideHUD();
        endScreen.Hide();

        startScreen.ShowStartScreen();
    }
    public void ShowHUD() {
        hudCanvas.ShowHUD();
        endScreen.Hide();

        startScreen.HideStartScreen();
    }

    public void ShowGameOver(bool hasWon) {
        hudCanvas.HideHUD();
        startScreen.HideStartScreen();

        endScreen.Show(hasWon);
    }
    public void ShowDialogueBox(DialogBoxData data) {
        dialogBoxController.ShowDialogBox(data);
    }
}