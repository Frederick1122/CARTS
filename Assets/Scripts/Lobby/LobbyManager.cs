using Base;
using CameraManger.Lobby;
using Lobby.Gacha;
using Lobby.Garage;
using Managers;
using Managers.Libraries;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : Singleton<LobbyManager>
{
    [SerializeField] private LobbyCameraManager _cameraManager;

    [field: Header("Parts")]
    [field: SerializeField] public Garage Garage { get; set; }
    [field: SerializeField] public LootBoxManager LootBoxManager { get; set; }

    public void Init()
    {
        _cameraManager.Create();
        _cameraManager.Init();

        Garage.Init();
        Garage.UpdateGarage(PlayerManager.Instance.GetCurrentCar());

        LootBoxManager.Init();
    }
}
