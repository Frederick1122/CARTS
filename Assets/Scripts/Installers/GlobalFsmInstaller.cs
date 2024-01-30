using Core.FSM;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class GlobalFsmInstaller : MonoInstaller
    {
        [SerializeField] private FsmManager _fsmManagerPrefab;

        public override void InstallBindings()
        {
            var fsmManager = Container.InstantiatePrefabForComponent<FsmManager>(_fsmManagerPrefab);
            fsmManager.Create();
            Container.Bind<FsmManager>().FromInstance(fsmManager).AsSingle().NonLazy();
        }
    }
}