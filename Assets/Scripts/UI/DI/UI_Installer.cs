using UnityEngine;
using Zenject;

public class UI_Installer : MonoInstaller {
    public override void InstallBindings() {
        //will use this later. No time to UI DI

        Container.DeclareSignal<TutorialSignal>().OptionalSubscriber();
    }
}
