using System;
using Zenject;

public class TutorialMachine : IInitializable {
    private readonly TutorialFactory _tutorialFactory;
    private readonly SignalBus _signalBus;

    private Tutorial _tutorial;
    
    public TutorialMachine(TutorialFactory tutorialFactory, SignalBus signalBus) {
        _tutorialFactory = tutorialFactory;
        _signalBus = signalBus;

        _signalBus.Subscribe<LevelStartedSignal>(OnLevelStarted);
    }
    public void Initialize() {
        ChangeState(Tutorial.Type.DASHBOARD);
    }

    private void OnLevelStarted(LevelStartedSignal signalData) {
        if(signalData.levelSettings.chapterID == 1) {
            ChangeState(Tutorial.Type.CHAPTER_1);
        }
    }


    public void ChangeState(Tutorial.Type type) {
        if(_tutorial != null) {
            _tutorial.Dispose();
            _tutorial = null;
        }
        _tutorial = _tutorialFactory.CreateFactory(type);

        UnityEngine.Debug.Log("Switching tutorial state to : " + type.ToString());
    }
}
