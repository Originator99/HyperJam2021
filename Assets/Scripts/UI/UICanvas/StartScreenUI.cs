using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class StartScreenUI : MonoBehaviour {

    private void OnEnable() {

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
