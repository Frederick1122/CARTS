using Core.FSM;
using ProjectFsms;
using Zenject;

namespace Installers
{
    public class FreeRideInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            FsmManager.Instance.SetActiveFsm<RaceFsm>();
        }
    }
}
