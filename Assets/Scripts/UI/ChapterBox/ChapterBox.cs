using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ChapterBox : MonoBehaviour {
    public Button startButton;
    public Transform levelParent;
    public GameObject levelIconPrefab;

    private ChapterSettingsInstaller.ChapterSettings _chapterSettings;
    private GameManager _gameManager;

    [Inject]
    public void Construct(ChapterSettingsInstaller.ChapterSettings chapterSettings, GameManager gameManager) {
        _chapterSettings = chapterSettings;
        _gameManager = gameManager;
    }

    public void RenderCurrentChapter(int chapter_id) {
        ChapterSO chapter = _chapterSettings.GetChapterByID(chapter_id);
        if(chapter != null) {
            RenderChaperLevels(chapter.levels);
        } else {
            Debug.LogError("Cannot find chapter with ID : " + chapter_id);
        }
    }

    private void RenderChaperLevels(List<Chapter> chapters) {
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

    public void SetStartButton(Chapter data) {
        startButton.onClick.RemoveAllListeners();
        startButton.onClick.AddListener(delegate() {
            _gameManager.BuildLevel(data.prefab_name);
        });
    }
}
