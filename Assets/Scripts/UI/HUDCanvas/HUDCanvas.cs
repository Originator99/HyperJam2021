using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class HUDCanvas : MonoBehaviour {
    public Pattern radarPattern;
    public DisplayScore score;

    public void ShowHUD() {
        radarPattern.Show();
        score.Show();
    }

    public void HideHUD() {
        radarPattern.Hide();
        score.Hide();
    }

}
