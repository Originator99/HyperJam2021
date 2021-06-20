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