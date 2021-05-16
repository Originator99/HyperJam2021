using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Chapter1Tutorials : Tutorial {
    private readonly Settings _settings;
    public Chapter1Tutorials(Settings settings, [Inject(Id ="brickCount")] RectTransform brickCount, [Inject(Id = "radar")] RectTransform radar) {
        _settings = settings;
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
