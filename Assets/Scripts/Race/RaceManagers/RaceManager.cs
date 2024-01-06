using Base;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RaceManager : Singleton<RaceManager>
{
    [SerializeField] protected CarController _playerPrefab;
    protected CarController _player;

    private void Start()
    {
        Init();
    }

    public virtual void Init() =>
        InitPlayer();

    protected abstract void InitPlayer();

    
    public abstract void StartRace();
}
