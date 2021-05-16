using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Chapter1Tutorials : Tutorial {
    private readonly SignalBus _signalBus;
    private readonly LevelManager _levelManager;
    private readonly Settings _settings;
    public Chapter1Tutorials(SignalBus signalBus, Settings settings, LevelManager levelManager, [Inject(Id ="brickCount")] RectTransform brickCount, [Inject(Id = "radar")] RectTransform radar) {
        _signalBus = signalBus;
        _settings = settings;
        _levelManager = levelManager;
    }

    public override void Start() {
        base.Start();
        CheckAndStartTutorial();
    }

    public override async void CheckAndStartTutorial() {
        await Task.Delay(500);
        Level.Settings levelSettings = _levelManager.GetCurrentLevelSettings();

        if(levelSettings.levelID == 1) {
            bool tutorial_complete = PlayerPrefs.GetInt("chapter_1_level_1_tutorial", 0) == 1;
            if(!tutorial_complete) {
                Level1Tutorial();
            }
        }
    }

    private void Level1Tutorial() {
        if(_settings != null && _settings.tutorialTexts != null) {
            List<TutorialData> sequence = new List<TutorialData>();

            List<TutorialText> tutorialTexts = FetchTutorialTexts(_settings.tutorialTexts, "c1_l1");
            if(tutorialTexts != null) {
                foreach(var tutText in tutorialTexts) {
                    TutorialData data = GenerateTutorialData(tutText.header, tutText.content);
                    sequence.Add(data);
                }
            }

            _signalBus.Fire<TutorialSignal>(new TutorialSignal { tutorials = sequence });
            PlayerPrefs.SetInt("chapter_1_level_1_tutorial", 1);
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
