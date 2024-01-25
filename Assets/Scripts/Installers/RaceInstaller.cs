using Core.FSM;
using FsmStates.RaceFsm;
using ProjectFsms;
using UI;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class RaceInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _managersParent;

        [Space(7)]
        [Header("Managers")]
        [SerializeField] private RaceManager _raceManagerPrefab;

        public override void InstallBindings()
        {
            Container.InstantiatePrefabForComponent<RaceManager>(_raceManagerPrefab);
            Container.Bind<IFsm>().To<RaceFsm>().AsSingle();
        }
    }
}