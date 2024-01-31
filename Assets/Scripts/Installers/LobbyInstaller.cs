using CameraManger.Lobby;
using Core.FSM;
using Lobby.Garage;
using ProjectFsms;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class LobbyInstaller : MonoInstaller
    {
        //[SerializeField] private GameObject _lobbyEnvironment;
        [SerializeField] private LobbyCameraManager _cameraManager;
        [SerializeField] private Garage _garage;

        public override void InstallBindings()
        {
            CameraManagerInstall();
            GarageInstall();
            FsmManager.Instance.SetActiveFsm<LobbyFsm>();
        }

        private void CameraManagerInstall()
        {
            _cameraManager.Create();
            _cameraManager.Init();
        }

        private void GarageInstall()
        {
            _garage.Init();
            Container.Bind<Garage>().FromInstance(_garage).AsSingle().NonLazy();
        }
    }
}