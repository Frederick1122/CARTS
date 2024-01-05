using Core.FSM;
using UnityEngine;
using Zenject;

public class LobbyInstaller : MonoInstaller
{
    [SerializeField] private GameObject _managersParent;

    [Space(7)] [Header("Managers")] [SerializeField]
    private FsmManager _fsmManagerPrefab;

    public override void InstallBindings()
    {
        var fsmManager =
            Container.InstantiatePrefabForComponent<FsmManager>(_fsmManagerPrefab, _managersParent.transform);
        Container.Bind<FsmManager>().FromInstance(fsmManager).AsSingle().NonLazy();
    }
}