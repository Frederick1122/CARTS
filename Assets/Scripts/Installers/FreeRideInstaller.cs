using Core.FSM;
using ProjectFsms;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class FreeRideInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Application.targetFrameRate = 75;

            FsmManager.Instance.SetActiveFsm<RaceFsm>();
        }
    }
}
