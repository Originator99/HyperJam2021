using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class HUDCanvas : MonoBehaviour {
    public GameObject radarPattern;
    public DisplayScore score;

    public void ShowHUD() {
        radarPattern.SetActive(true);
        score.Show();
    }

    public void HideHUD() {
        radarPattern.SetActive(false);
        score.Hide();
    }

}
