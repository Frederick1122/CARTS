using Core.FSM;
using ProjectFsms;
using Zenject;

namespace Installers
{
    public class RaceInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            FsmManager.Instance.SetActiveFsm<RaceFsm>();
        }
    }
}