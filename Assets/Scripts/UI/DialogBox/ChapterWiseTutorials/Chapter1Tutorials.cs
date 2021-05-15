using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Chapter1Tutorials : Tutorial {
    private readonly Settings _settings;
    public Chapter1Tutorials(Settings settings, [Inject(Id ="brickCount")] Transform brickCount, [Inject(Id = "radar")] Transform radar) {
        _settings = settings;
    }


    #region Settings and Factory
    public class Settings {
        public List<TutorialText> tutorialTexts;
    }
    public class Factory :PlaceholderFactory<Chapter1Tutorials> {

    }
    #endregion
}
