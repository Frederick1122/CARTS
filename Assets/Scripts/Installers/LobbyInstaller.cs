using Core.FSM;
using ProjectFsms;
using Zenject;


namespace Installers
{
    public class LobbyInstaller : MonoInstaller
    {
        public override void InstallBindings() =>
            Container.Bind<IFsm>().To<LobbyFsm>().AsSingle();
    }
}