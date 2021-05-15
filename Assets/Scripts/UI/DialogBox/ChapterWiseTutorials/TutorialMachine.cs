using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TutorialMachine : MonoBehaviour {
    private TutorialFactory _tutorialFactory;
    private Tutorial _tutorial;
    private SignalBus _signalBus;

    [Inject]
    public void Construct(TutorialFactory tutorialFactory, SignalBus signalBus) {
        _tutorialFactory = tutorialFactory;
        _signalBus = signalBus;
    }

    private void Start() {
        
    }

    public void ChangeState(Tutorial.Type type) {
        if(_tutorial != null) {
            _tutorial.Dispose();
            _tutorial = null;
        }
        _tutorial = _tutorialFactory.CreateFactory(type);
    }
}
