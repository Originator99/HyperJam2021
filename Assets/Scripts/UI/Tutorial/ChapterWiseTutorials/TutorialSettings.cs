using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[CreateAssetMenu(menuName = "Roulette/Tutorial Settings")]
public class TutorialSettings :ScriptableObjectInstaller<TutorialSettings> {
    public override void InstallBindings() {
        Container.BindInstance(tutorialManagerSettings);
        Container.BindInstance(dashboardTutorialSettings);
        Container.BindInstance(chapter1TutorialSettings);
    }

    public TutorialManager.Settings tutorialManagerSettings;
    public DashboardTutorials.Settings dashboardTutorialSettings;
    public Chapter1Tutorials.Settings chapter1TutorialSettings;

}
