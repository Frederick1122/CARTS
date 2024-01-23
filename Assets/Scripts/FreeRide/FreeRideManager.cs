using Cars.Controllers;
using FreeRide;
using Managers;
using Managers.Libraries;
using UnityEngine;

public class FreeRideManager : RaceManager
{
    private const string PLAYER_PRESET_NAME = "PlayerPreset";

    [SerializeField] private Transform _startPosition;
    [SerializeField] private MapFabric _mapFabric;

    // temp
    private void Start()
    {
        Init();
        StartRace();
    }

    public override void Init()
    {
        _mapFabric.Init();
        base.Init();
    }

    public override void StartRace() =>
        _player.StartCar();

    protected override void InitPlayer()
    {
        var playerConfig = CarLibrary.Instance.GetConfig(PlayerManager.Instance.GetCurrentCar().configKey);
        var playerPreset = PresetLibrary.Instance.GetConfig(PLAYER_PRESET_NAME);
        var playerPrefab = playerConfig.prefab;
        var player = Instantiate(playerPrefab, _startPosition);
        _player = (CarController)player.gameObject.AddComponent(playerPreset.CarController);

        var playerInputSystem = (IInputSystem)_player.gameObject.AddComponent(playerPreset.InputSystem);
        playerInputSystem.Init(playerPreset, playerPrefab);

        _player.Init(playerInputSystem, playerConfig, playerPreset);
    }


}
