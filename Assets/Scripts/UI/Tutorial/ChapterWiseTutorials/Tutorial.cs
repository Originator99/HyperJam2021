using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public abstract class Tutorial :IDisposable {
    public enum Type {
        DASHBOARD,
        CHAPTER_SELECTION_PAGE,
        CHAPTER_1
    }
    public virtual void Dispose() {
    }

    public virtual void Start() {
        // optionally overridden
    }

    public abstract void CheckAndStartTutorial();

    protected TutorialData GenerateTutorialData(string header, string content, List<UnityEngine.Transform> highlightPanels = null, System.Action callback = null) {
        TutorialData data = new TutorialData() { 
            header = header,
            content = content,
            highLightPanels = highlightPanels,
            callback = callback,
        };
        return data;
    }

    protected List<TutorialText> FetchTutorialTexts(List<TutorialText> tutorialTexts, string id) {
        if(tutorialTexts != null && !string.IsNullOrEmpty(id)) {
            return tutorialTexts.FindAll(x => x.id == id).OrderBy(y => y.sequenceNumber).ToList();
        }
        return null;
    }
}
