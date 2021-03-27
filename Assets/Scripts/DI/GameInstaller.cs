using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller {
    public override void InstallBindings() {
        InstallPlayer();
        Container.BindInterfacesAndSelfTo<LevelManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameManager>().AsSingle();
    }

    private void InstallPlayer() {
        Container.Bind<PlayerStateFactory>().AsSingle();

        Container.BindFactory<PlayerStateWaitingToStart, PlayerStateWaitingToStart.Factory>().WhenInjectedInto<PlayerStateFactory>();
        Container.BindFactory<PlayerStateMoving, PlayerStateMoving.Factory>().WhenInjectedInto<PlayerStateFactory>();

    }

    private void InstallExcecutionOrder() {
        //We want LevelManager to create our level first then allow users to move their players
        Container.BindExecutionOrder<LevelManager>(-10);
        Container.BindExecutionOrder<GameManager>(-20);
    }
}
