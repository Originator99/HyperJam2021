using UnityEngine;
using Zenject;

public class TutorialInstaller : MonoInstaller {
    public override void InstallBindings() {
        Container.Bind<TutorialFactory>().AsSingle();
        Container.BindInterfacesAndSelfTo<TutorialMachine>().AsSingle();

        Container.BindFactory<DashboardTutorials, DashboardTutorials.Factory>().WhenInjectedInto<TutorialFactory>();
        Container.BindFactory<Chapter1Tutorials, Chapter1Tutorials.Factory>().WhenInjectedInto<TutorialFactory>();
    }
}

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