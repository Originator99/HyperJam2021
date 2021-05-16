using ModestTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFactory {

    private readonly DashboardTutorials.Factory _dashbaordTutorials;
    private readonly Chapter1Tutorials.Factory _chapter1Tutorials;

    public TutorialFactory(Chapter1Tutorials.Factory chapter1Tutorials, DashboardTutorials.Factory dashbaordTutorials) {
        _dashbaordTutorials = dashbaordTutorials;
        _chapter1Tutorials = chapter1Tutorials;
    }

    public Tutorial CreateFactory(Tutorial.Type type) {
        switch(type) {
            case Tutorial.Type.CHAPTER_1:
                return _chapter1Tutorials.Create();
            case Tutorial.Type.DASHBOARD:
                return _dashbaordTutorials.Create();

        }
        throw Assert.CreateException();
    }

}
