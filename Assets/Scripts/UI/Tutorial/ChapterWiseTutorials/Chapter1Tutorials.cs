using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;

public class Chapter1Tutorials : Tutorial {
    private readonly SignalBus _signalBus;
    private readonly LevelManager _levelManager;
    private readonly Settings _settings;
    private readonly SpriteRenderer _playerSprite;
    public Chapter1Tutorials(SignalBus signalBus, Settings settings, LevelManager levelManager, [Inject(Id ="brickCount")] RectTransform brickCount, [Inject(Id = "radar")] RectTransform radar, [Inject(Id = "playerSprite")] SpriteRenderer playerSprite) {
        _signalBus = signalBus;
        _settings = settings;
        _levelManager = levelManager;
        _playerSprite = playerSprite;

        _signalBus.Subscribe<PlayerReachedEndSignal>(OnLevelEnd);
    }

    public override void Start() {
        base.Start();
        CheckAndStartTutorial();
    }

    private void OnLevelEnd(PlayerReachedEndSignal signalData) {
        CheckForLevelEndTutorials(signalData.hasWon);
    }

    public override async void CheckAndStartTutorial() {
        Level.Settings levelSettings = _levelManager.GetCurrentLevelSettings();

        if(levelSettings.levelID == 1) {
            bool tutorial_complete = PlayerPrefs.GetInt("chapter_1_level_1_tutorial", 0) == 1;
            if(!tutorial_complete) {
                await Task.Delay(500);
                Level1Tutorial();
            }
        }
    }

    private async void CheckForLevelEndTutorials(bool hasWon) {
        Level.Settings levelSettings = _levelManager.GetCurrentLevelSettings();
        if(levelSettings.levelID == 1 && hasWon) {
            bool tutorial_complete = PlayerPrefs.GetInt("chapter_1_level_1_end_tutorial", 0) == 1;
            if(!tutorial_complete) {
                await Task.Delay(1500);
                Level1EndTutorial();
            }
        }
    }

    private void Level1Tutorial() {
        if(_settings != null && _settings.tutorialTexts != null) {
            List<TutorialData> sequence = new List<TutorialData>();

            List<TutorialText> tutorialTexts = FetchTutorialTexts(_settings.tutorialTexts, "c1_l1_1");
            if(tutorialTexts != null) {
                foreach(var tutText in tutorialTexts) {
                    List<Transform> highlight = new List<Transform> { _playerSprite.transform };
                    if(tutText.sequenceNumber == 7) {
                        List<BaseBrick> portals = _levelManager.GetPortalBricks();
                        if(portals != null && portals.Count > 0) {
                            highlight.Add(portals[0].transform);
                        }
                    }
                    TutorialData data = GenerateTutorialData(tutText.header, tutText.content, highlight);
                    sequence.Add(data);
                }
            }

            _signalBus.Fire<TutorialSignal>(new TutorialSignal { tutorials = sequence });
           // PlayerPrefs.SetInt("chapter_1_level_1_tutorial", 1);
        }
    }

    private void Level1EndTutorial() {
        if(_settings != null && _settings.tutorialTexts != null) {
            List<TutorialData> sequence = new List<TutorialData>();

            List<TutorialText> tutorialTexts = FetchTutorialTexts(_settings.tutorialTexts, "c1_l1_end");
            if(tutorialTexts != null) {
                foreach(var tutText in tutorialTexts) {
                    TutorialData data = GenerateTutorialData(tutText.header, tutText.content, null);
                    sequence.Add(data);
                }
            }
            _signalBus.Fire<TutorialSignal>(new TutorialSignal { tutorials = sequence });
            PlayerPrefs.SetInt("chapter_1_level_1_end_tutorial", 1);
        }
    }

    #region Settings and Factory
    [System.Serializable]
    public class Settings {
        public List<TutorialText> tutorialTexts;
    }
    public class Factory :PlaceholderFactory<Chapter1Tutorials> {

    }
    #endregion
}
