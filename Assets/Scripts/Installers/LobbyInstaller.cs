using Core.FSM;
using ProjectFsms;
using Zenject;


namespace Installers
{
    public class LobbyInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            FsmManager.Instance.SetActiveFsm<LobbyFsm>();
        }
    }
}