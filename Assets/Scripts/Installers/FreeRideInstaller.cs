using UnityEngine;
using Zenject;

namespace Installers
{
    public class FreeRideInstaller : MonoInstaller
    {
        [SerializeField] private Transform _managersParent;

        [Space(7)]
        [Header("Managers")]
        [SerializeField] private RaceManager _raceManagerPrefab;

        public override void InstallBindings()
        {
            var raceManager = Container.InstantiatePrefabForComponent<RaceManager>(_raceManagerPrefab);
        }
    }
}
