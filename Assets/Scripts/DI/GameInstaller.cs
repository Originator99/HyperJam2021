using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller {
    public override void InstallBindings() {
        SignalBusInstaller.Install(Container);
        InstallPlayer();
        InstallLevel();
        InstallCamera();
        InstallExcecutionOrder();

        Container.BindInterfacesAndSelfTo<GameManager>().AsSingle();
        Container.DeclareSignal<PlayerDiedSignal>();
    }

    private void InstallPlayer() {
        Container.Bind<PlayerStateFactory>().AsSingle();

        Container.BindFactory<PlayerStateWaitingToStart, PlayerStateWaitingToStart.Factory>().WhenInjectedInto<PlayerStateFactory>();
        Container.BindFactory<PlayerStateMoving, PlayerStateMoving.Factory>().WhenInjectedInto<PlayerStateFactory>();
        Container.BindFactory<PlayerStateDash, PlayerStateDash.Factory>().WhenInjectedInto<PlayerStateFactory>();
        Container.BindFactory<PlayerStateDead, PlayerStateDead.Factory>().WhenInjectedInto<PlayerStateFactory>();
    }

    private void InstallLevel() {
        Container.BindInterfacesAndSelfTo<LevelManager>().AsSingle();
        Container.Bind<LevelRandomizer>().AsSingle();
    }

    private void InstallCamera() {
        Container.Bind<CameraStateFactory>().AsSingle();

        Container.BindFactory<CameraStateFollowing, CameraStateFollowing.Factory>().WhenInjectedInto<CameraStateFactory>();
    }

    private void InstallExcecutionOrder() {
        //We want LevelManager to create our level first then allow users to move their players
        Container.BindExecutionOrder<LevelManager>(-10);
        Container.BindExecutionOrder<GameManager>(-20);
    }
}
