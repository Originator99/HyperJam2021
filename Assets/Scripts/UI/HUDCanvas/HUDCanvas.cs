using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class HUDCanvas : MonoBehaviour {
    public GameObject radarPattern;
    public DisplayScore score;

    public void ShowHUD() {
        gameObject.SetActive(true);
    }

    public void HideHUD() {
        gameObject.SetActive(false);
    }

}
