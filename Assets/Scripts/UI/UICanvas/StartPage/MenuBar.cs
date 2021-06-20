using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuBar : MonoBehaviour {

    public ChapterBox chapterBox;
    public ControlsBox controlBox;

    public Button chapterBtn, shopBtn, controlsBtn, settingsBtn;

    public Transform menuItemHighlight;

    private IMenuItem currentController;
    private MenuItem currentMenuItem;

    private enum MenuItem {
        CHAPTER,
        SHOP,
        CONTROLS,
        SETTINGS,
        NONE,
    }

    private void Start() {
        currentMenuItem = MenuItem.NONE;

        chapterBtn.onClick.RemoveAllListeners();
        chapterBtn.onClick.AddListener(delegate() {
            SwitchToChapter();
        });

        shopBtn.onClick.RemoveAllListeners();
        shopBtn.onClick.AddListener(delegate() {
            SwitchToShop();
        });

        controlsBtn.onClick.RemoveAllListeners();
        controlsBtn.onClick.AddListener(delegate() {
            SwitchToControls();
        });

        settingsBtn.onClick.RemoveAllListeners();
        settingsBtn.onClick.AddListener(delegate() {
            SwitchToSettings();
        });
    }

    private void SwitchToChapter() {
        if(currentMenuItem != MenuItem.CHAPTER) {
            currentMenuItem = MenuItem.CHAPTER;
            menuItemHighlight.DOMove(chapterBtn.transform.position, 0.15f);
            HideCurrent(delegate() {
                currentController = chapterBox.GetComponent<IMenuItem>();
                ShowCurrent();
            });
        }
    }
    
    private void SwitchToShop() {
        if(currentMenuItem != MenuItem.SHOP) {
            currentMenuItem = MenuItem.SHOP;
            menuItemHighlight.DOMove(shopBtn.transform.position, 0.15f);
            HideCurrent(null);
        }
    }

    private void SwitchToControls() {
        if(currentMenuItem != MenuItem.CONTROLS) {
            currentMenuItem = MenuItem.CONTROLS;
            menuItemHighlight.DOMove(controlsBtn.transform.position, 0.15f);
            HideCurrent(delegate() {
                currentController = controlBox.GetComponent<IMenuItem>();
                ShowCurrent();
            });
        }
    }

    private void SwitchToSettings() {
        if(currentMenuItem != MenuItem.SETTINGS) {
            currentMenuItem = MenuItem.SETTINGS;
            menuItemHighlight.DOMove(settingsBtn.transform.position, 0.15f);
            HideCurrent(null);
        }
    }

    private void HideCurrent(System.Action callback) {
        if(currentController != null) {
            currentController.Hide(callback);
            currentController = null;
        } else {
            callback?.Invoke();
            Debug.LogError("Controller is null or not inheritted, cannot hide");
        }
    }
    private void ShowCurrent() {
        if(currentController != null) {
            currentController.Show();
        } else {
            Debug.LogError("Controller is null or not inheritted, cannot show");
        }
    }
}
