using Base;
using Cars.Controllers;
using UnityEngine;

public abstract class RaceManager : Singleton<RaceManager>
{
    protected CarController _player;

    public virtual void Init() =>
        InitPlayer();

    protected abstract void InitPlayer();

    
    public abstract void StartRace();
}
