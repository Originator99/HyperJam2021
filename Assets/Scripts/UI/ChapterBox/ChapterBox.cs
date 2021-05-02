using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

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

    private void RenderChaperLevels(List<LevelData> chapters) {
        if(chapters != null) {
            int new_icons = chapters.Count - levelParent.childCount;
            for(int i = 0; i < new_icons; i++) {
                Instantiate(levelIconPrefab, levelParent);
            }

            int index = 0;
            for(int i = 0; i < levelParent.childCount; i++) {
                Transform child = levelParent.GetChild(i);
                if(index < chapters.Count) {
                    ChapterIcon controller = child.GetComponent<ChapterIcon>();
                    if(controller != null) {
                        controller.RenderIcon(chapters[index], 1);
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

    public void SetStartButton(LevelData data) {
        startButton.onClick.RemoveAllListeners();
        startButton.onClick.AddListener(delegate() {
            _gameManager.BuildLevel(data);
        });
    }
}
