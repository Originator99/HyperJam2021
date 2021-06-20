using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ChapterIcon :MonoBehaviour {
    public Button button;
    public TMP_Text level_number;
    public GameObject activeState, lockedState, completeState;

    public System.Action<LevelData> OnSelected;

    public void RenderIcon(LevelData data, int current_level_number) {
        level_number.text = data.level_number.ToString();

        bool isLocked = false;
        if(data.is_complete) {
            SetAsComplete();
        } else {
            if(data.level_number == current_level_number) {
                SetAsActive();
            } else {
                isLocked = true;
                SetAsLocked();
            }
        }
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(delegate() {
            //if(isLocked || data.is_complete) {
            //    lockedState.transform.DOShakePosition(0.15f, new Vector3(10, 0), 50, 0);
            //} else {
            //    OnSelected(data);
            //}
            OnSelected(data);
        });
        gameObject.SetActive(true);
        DoAnimation();
    }

    public void SetAsActive() {
        activeState.SetActive(true);
        lockedState.SetActive(false);
        completeState.SetActive(false);
    }

    public void SetAsLocked() {
        activeState.SetActive(false);
        lockedState.SetActive(true);
        completeState.SetActive(false);
    }

    public void SetAsComplete() {
        activeState.SetActive(false);
        lockedState.SetActive(false);
        completeState.SetActive(true);
    }

    private void DoAnimation() {
        //transform.localScale = new Vector3(0, 0, 0);
        //transform.DOScale(new Vector3(1,1,1), 0.5f);
    }
}
