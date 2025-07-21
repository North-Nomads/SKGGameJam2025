using HighVoltage.Infrastructure.AssetManagement;
using HighVoltage.Infrastructure.CameraService;
using HighVoltage.Infrastructure.Factory;
using HighVoltage.Infrastructure.SaveLoad;
using HighVoltage.Infrastructure.Services;
using HighVoltage.Services.Progress;
using HighVoltage.UI.Services.Factory;
using HighVoltage.UI.Services.Windows;
using System;
using System.Collections.Generic;
using HighVoltage.Infrastructure.BuildingStore;
using HighVoltage.Infrastructure.InGameTime;
using UnityEngine;
using HighVoltage.Infrastructure.MobSpawning;
using HighVoltage.Infrastructure.Tutorial;
using HighVoltage.Level;
using HighVoltage.StaticData;
using HighVoltage.UI.Services;
using HighVoltage.Map.Building;
using Unity.VisualScripting;

namespace HighVoltage.Infrastructure.States
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;
        private IExitableState _currentState;

        public GameStateMachine(SceneLoader sceneLoader, Canvas loadingCurtain, AllServices services,
            ICoroutineRunner coroutineRunner)
        {
            _states = new Dictionary<Type, IExitableState>()
            {
                [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader, services, coroutineRunner),
                [typeof(LoadProgressState)] = new LoadProgressState(this,
                    services.Single<IPlayerProgressService>(),
                    services.Single<ISaveLoadService>()),
                [typeof(HubState)] = new HubState(this,
                    sceneLoader,
                    loadingCurtain,
                    services.Single<IGameFactory>(),
                    services.Single<IPlayerProgressService>(),
                    services.Single<IUIFactory>(),
                    services.Single<IWindowService>(),
                    services.SaveReaderServices,
                    services.Single<ICameraService>(),
                    services.Single<IAssetProvider>()),
                [typeof(LoadLevelState)] = new LoadLevelState(this,
                    sceneLoader,
                    loadingCurtain,
                    services.Single<IGameFactory>(),
                    services.Single<IPlayerProgressService>(),
                    services.Single<IMobSpawnerService>(),
                    services.Single<IStaticDataService>(),
                    services.Single<IGameWindowService>(),
                    services.Single<IUIFactory>(),
                    services.Single<IPlayerBuildingService>(),
                    services.Single<ILevelProgress>(),
                    services.Single<IBuildingStoreService>(),
                    services.Single<ICameraService>()),
                [typeof(GameLoopState)] = new GameLoopState(this,
                    services.Single<ISaveLoadService>(),
                    services.Single<IGameWindowService>(),
                    services.Single<ILevelProgress>(),
                    services.Single<IMobSpawnerService>(),
                    services.Single<IPlayerBuildingService>()),
                [typeof(GameFinishedState)] = new GameFinishedState(this,
                    services.Single<IPlayerProgressService>(),
                    services.Single<IGameWindowService>(),
                    services.Single<IPlayerProgressService>(),
                    services.Single<IInGameTimeService>()),
                [typeof(TutorialLoopState)] = new TutorialLoopState(this, services.Single<ITutorialService>()),
                [typeof(LoadTutorialState)] = new LoadTutorialState(this,
                    services.Single<IUIFactory>(),
                    loadingCurtain, 
                    services.Single<IStaticDataService>(),
                    services.Single<ITutorialService>(),
                    sceneLoader,
                    services.Single<ICameraService>(),
                    services.Single<IGameWindowService>(),
                    services.Single<IPlayerBuildingService>(),
                services.Single<IBuildingStoreService>(),
                    services.Single<ILevelProgress>(),
                    services.Single<IPlayerProgressService>(),
                    services.Single<IGameFactory>(),
                    services.Single<IMobSpawnerService>()),
            };
        }

        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            // The first state could be null on programm start 
            _currentState?.Exit();
            TState state = GetState<TState>();
            _currentState = state;
            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState
            => _states[typeof(TState)] as TState;
    }
}