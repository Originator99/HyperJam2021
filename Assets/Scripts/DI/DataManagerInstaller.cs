using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DataManagerInstaller : MonoInstaller {
    public override void InstallBindings() {
        Container.BindInterfacesAndSelfTo<OfflineDataManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<ChapterDataManager>().AsSingle();

        Container.BindExecutionOrder<ChapterDataManager>(-10);
    }
}
