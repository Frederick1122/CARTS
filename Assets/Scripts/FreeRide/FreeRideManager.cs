using Cars.Controllers;
using Core.FSM;
using FreeRide;
using Managers;
using Managers.Libraries;
using System;
using UnityEngine;

public class FreeRideManager : RaceManager
{
    private const string PLAYER_PRESET_NAME = "PlayerPreset";

    public event Action<int> OnResultUpdate;
    public event Action OnFinish;


    [SerializeField] private Transform _startPosition;
    [SerializeField] private MapFabric _mapFabric;
    [SerializeField] private DifficultyModifier _difficultyModifier;

    // temp
    //private void Start()
    //{
    //    Init();
    //    StartRace();
    //}

    public override void Init()
    {
        _mapFabric.Init();
        _mapFabric.OnResultUpdate += UpdateResult;
        _mapFabric.OnFall += PlayerFall;

        base.Init();

        _difficultyModifier.Init(_player, this);
    }

    private void OnDestroy()
    {
        _mapFabric.OnResultUpdate -= UpdateResult;
        _mapFabric.OnFall -= PlayerFall;
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

        var playerInputSystem = (IInputSystem)_player.gameObject.AddComponent(typeof(FreeRideInputSystem));
        playerInputSystem.Init(playerPreset, playerPrefab);

        _player.Init(playerInputSystem, playerConfig, playerPreset);
    }


    private void PlayerFall()
    {
        _player.StopCar();
        OnFinish?.Invoke();
    }

    private void UpdateResult(int val)
    {
        Debug.Log($"Result {val}");
        OnResultUpdate?.Invoke(val);
    }
}
