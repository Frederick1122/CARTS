using UI;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class GlobalUiInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _uiPrefab;

        public override void InstallBindings()
        {
            var uiPrefab = Container.InstantiatePrefab(_uiPrefab);

            var uiManager = uiPrefab.GetComponentInChildren<UIManager>();
            uiManager.Create();
            uiManager.Init();
            Container.Bind<UIManager>().FromInstance(uiManager).AsSingle().NonLazy();
        }
    }
}