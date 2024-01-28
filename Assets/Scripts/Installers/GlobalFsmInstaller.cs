using Core.FSM;
using ProjectFsms;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class GlobalFsmInstaller : MonoInstaller
    {
        [SerializeField] private FsmManager _fsmManagerPrefab;

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<FsmRegisterHandler>().AsSingle().CopyIntoDirectSubContainers();

            var fsmManager = Container.InstantiatePrefabForComponent<FsmManager>(_fsmManagerPrefab);
            Container.Bind<FsmManager>().FromInstance(fsmManager).AsSingle().NonLazy();
        }
    }
}