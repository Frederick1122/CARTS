using Core.FSM;
using FsmStates.RaceFsm;
using ProjectFsms;
using UI;
using UnityEngine;
using Zenject;

namespace CustomSnapTool.Installers
{
    public class RaceInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _managersParent;

        [Space(7)] [Header("Managers")] [SerializeField]
        private FsmManager _fsmManagerPrefab;
        [SerializeField] private RaceManager _raceManagerPrefab;

        [Space(7)] [Header("Prefabs")] [SerializeField]
        private GameObject _uiPrefab;
    
        public override void InstallBindings()
        {
            var raceManager = Container.InstantiatePrefabForComponent<RaceManager>(_raceManagerPrefab);
            var uiPrefab = Container.InstantiatePrefab(_uiPrefab);
            var lobbyUIManager = uiPrefab.GetComponentInChildren<LobbyUIManager>();
            lobbyUIManager.Init();
            Container.Bind<LobbyUIManager>().FromInstance(lobbyUIManager).AsSingle().NonLazy();
        
            var fsmManager =
                Container.InstantiatePrefabForComponent<FsmManager>(_fsmManagerPrefab, _managersParent.transform);
            Container.Bind<FsmManager>().FromInstance(fsmManager).AsSingle().NonLazy();

            var raceFsm = new RaceFsm(raceManager); 
            fsmManager.AddNewFsm(raceFsm);
            fsmManager.TryGetFsm<RaceFsm>().SetState<PreInitializeState>();
        }
    }
}