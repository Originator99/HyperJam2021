using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DashboardTutorials : Tutorial {

    #region Settings and Factory
    [System.Serializable]
    public class Settings {
        public List<TutorialText> tutorialTexts;
    }
    public class Factory :PlaceholderFactory<DashboardTutorials> {

    }
    #endregion

}
