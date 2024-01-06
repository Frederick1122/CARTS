using Core.FSM;
using FsmStates.LobbyFsm;
using ProjectFsms;
using UI;
using UnityEngine;
using Zenject;

public class LobbyInstaller : MonoInstaller
{
    [SerializeField] private GameObject _managersParent;

    [Space(7)] [Header("Managers")] [SerializeField]
    private FsmManager _fsmManagerPrefab;

    [Space(7)] [Header("Prefabs")] [SerializeField]
    private GameObject _uiPrefab;
    
    public override void InstallBindings()
    {
        var uiPrefab = Container.InstantiatePrefab(_uiPrefab);
        var lobbyUIManager = uiPrefab.GetComponentInChildren<LobbyUIManager>();
        lobbyUIManager.Init();
        Container.Bind<LobbyUIManager>().FromInstance(lobbyUIManager).AsSingle().NonLazy();
        
        var fsmManager =
            Container.InstantiatePrefabForComponent<FsmManager>(_fsmManagerPrefab, _managersParent.transform);
        Container.Bind<FsmManager>().FromInstance(fsmManager).AsSingle().NonLazy();

        var lobbyFsm = new LobbyFsm(lobbyUIManager); 
        fsmManager.AddNewFsm(lobbyFsm);
        fsmManager.TryGetFsm<LobbyFsm>().SetState<PreInitializeState>();
    }
}