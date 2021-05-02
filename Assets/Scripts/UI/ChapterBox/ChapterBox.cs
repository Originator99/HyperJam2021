using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using DG.Tweening;

public class ChapterBox : MonoBehaviour {
    public Button startButton;
    public Transform levelParent;
    public GameObject levelIconPrefab;

    private ChapterDataManager _chapterDataManager;
    private GameManager _gameManager;

    [Inject]
    public void Construct(ChapterDataManager chapterDataManager, GameManager gameManager) {
        _chapterDataManager = chapterDataManager;
        _gameManager = gameManager;
    }

    public void RenderCurrentChapter() {
        ChapterData chapter = _chapterDataManager.GetCurrentChapter();
        if(chapter != null) {
            RenderChaperLevels(chapter.levels);
        } else {
            Debug.LogError("Cannot find chapter ");
        }
    }

    private void RenderChaperLevels(List<LevelData> levels) {
        if(levels != null) {
            int new_icons = levels.Count - levelParent.childCount;
            for(int i = 0; i < new_icons; i++) {
                Instantiate(levelIconPrefab, levelParent);
            }

            int index = 0;
            for(int i = 0; i < levelParent.childCount; i++) {
                Transform child = levelParent.GetChild(i);
                if(index < levels.Count) {
                    ChapterIcon controller = child.GetComponent<ChapterIcon>();
                    if(controller != null) {
                        controller.RenderIcon(levels[index], GetActiveLevel(levels));
                        controller.OnSelected += SetStartButton;
                    }
                    index++;
                } else {
                    child.gameObject.SetActive(false);
                }
            }
        } else {
            Debug.LogError("Level prefabs are null");
        }
    }

    private int GetActiveLevel(List<LevelData> levels) {
        if(levels != null) {
            LevelData data = levels.Find(x => !x.is_complete);
            if(data != null) {
                return data.level_number;
            }
        }
        return 1;
    }

    public void SetStartButton(LevelData data) {
        startButton.interactable = false;
        startButton.transform.DOScale(new Vector3(0, 0, 0), 0.15f).OnComplete(delegate () {
            startButton.transform.DOScale(new Vector3(1, 1, 1), 0.15f).OnComplete(delegate() {
                startButton.interactable = true;
            });
        });

        startButton.onClick.RemoveAllListeners();
        startButton.onClick.AddListener(delegate() {
            _gameManager.BuildLevel(data);
        });
    }
}
