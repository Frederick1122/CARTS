using Base;
using UnityEngine;

public abstract class RaceManager : Singleton<RaceManager>
{
    [SerializeField] protected CarController _playerPrefab;
    protected CarController _player;

    public virtual void Init() =>
        InitPlayer();

    protected abstract void InitPlayer();

    
    public abstract void StartRace();
}
