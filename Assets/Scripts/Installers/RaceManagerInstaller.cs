using Race.RaceManagers;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class RaceManagerInstaller : MonoInstaller
    {
        [SerializeField] private RaceManager _raceManagerPrefab;

        public override void InstallBindings()
        {
            var raceManager = Container.InstantiatePrefabForComponent<RaceManager>(_raceManagerPrefab);
            raceManager.Create();
            raceManager.Init();
            Container.Bind<RaceManager>().FromInstance(raceManager).AsSingle().NonLazy();
        }
    }
}