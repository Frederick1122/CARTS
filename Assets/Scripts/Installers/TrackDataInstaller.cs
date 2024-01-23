using System;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class TrackDataInstaller : MonoInstaller
    {
        [SerializeField] private TrackData _defaultTrackData;

        public override void InstallBindings()
        {
            Container.Bind<TrackData>().FromInstance(_defaultTrackData).AsSingle().NonLazy();
        }
    }

    [Serializable]
    public class TrackData
    {
        public string configKey;
    }
}