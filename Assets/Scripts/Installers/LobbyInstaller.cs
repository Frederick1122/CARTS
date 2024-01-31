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
        [SerializeField] private LobbyManager _manager;

        public override void InstallBindings()
        {
            CreateLobbyManager();
            FsmManager.Instance.SetActiveFsm<LobbyFsm>();
        }

        private void CreateLobbyManager()
        {
            var manager = Container.InstantiatePrefabForComponent<LobbyManager>(_manager);
            manager.Create();
            manager.Init();
        }
    }
}