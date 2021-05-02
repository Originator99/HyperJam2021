using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChapterIcon :MonoBehaviour {
    public Button button;
    public TMP_Text level_number;
    public GameObject activeState, lockedState, completeState;

    public System.Action<LevelData> OnSelected;

    public void RenderIcon(LevelData data, int current_level_number) {
        gameObject.SetActive(true);

        level_number.text = data.level_number.ToString();

        if(data.is_complete) {
            SetAsComplete();
        } else {
            if(data.level_number == current_level_number) {
                SetAsActive();
            } else {
                SetAsLocked();
            }
        }
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(delegate() {
            OnSelected(data);
        });
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
}
