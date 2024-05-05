using Core.Data;
using Core.FSM;
using FsmStates.LobbyFsm;
using Installers;
using Lobby.Garage;
using UI;
using Zenject;

namespace ProjectFsms
{
    public class LobbyFsm : Fsm
    {
        private const string FIRST_OPEN_KEY = "FIRST_OPEN_LOBBY";

        private LobbyUI _lobbyUI;

        [Inject] public GameDataInstaller.GameData gameData;
        public bool IsFirstOpen { get; private set; } = true;

        private Garage _garage => LobbyManager.Instance.Garage;

        public override void Init()
        {
            SetFirstOpen();

            _lobbyUI = UIManager.Instance.GetLobbyUi();

            _states.Add(typeof(PreInitializeState), new PreInitializeState(this));
            _states.Add(typeof(LobbyState), new LobbyState(this, _lobbyUI));
            _states.Add(typeof(ShopState), new ShopState(this, _lobbyUI, _garage));
            _states.Add(typeof(GarageState), new GarageState(this));
            _states.Add(typeof(SettingsState), new SettingsState(this, _lobbyUI));
            _states.Add(typeof(MapSelectionState), new MapSelectionState(this, _lobbyUI));
            _states.Add(typeof(StartGameState), new StartGameState(this));

            base.Init();
        }

        private void SetFirstOpen()
        {
            if (PlayerPrefsSaveLoadManager.HasKey(FIRST_OPEN_KEY))
                IsFirstOpen = false;
            else
                PlayerPrefsSaveLoadManager.SaveInt(FIRST_OPEN_KEY, 1);
        }
    }
}