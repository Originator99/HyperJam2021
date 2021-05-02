using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class StartScreenUI : MonoBehaviour {
    public ChapterBox chapterBox;

    public Button startButton;

    private void OnEnable() {
        Invoke(nameof(RenderChapterBox), 1f);
    }

    private void RenderChapterBox() {   
        chapterBox.RenderCurrentChapter();
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
