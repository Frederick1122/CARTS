using System.Collections.Generic;
using UnityEngine;

public class LapRaceManager : RaceManager
{
    [SerializeField] private int _playerPlace = -1;

    [SerializeField] private List<StartRacePlace> _carPlaces = new(4);
    [SerializeField] private List<CarController> _aiPrefabs = new();

    protected List<CarController> _enemies = new();

    public override void Init()
    {
        base.Init();
        InitAi();
    }

    public override void StartRace()
    {
        foreach (var en in _enemies)
            en.StartCar();

        _player.StartCar();
    }

    [ContextMenu("Test Race")]
    public void TestRace()
    {
        Init();
        StartRace();
    }

    protected override void InitPlayer()
    {
        var skipPlace = _playerPlace == -1 ? _carPlaces.Count - 1 : _playerPlace;

        _player = Instantiate(_playerPrefab, _carPlaces[skipPlace].transform);
        _player.transform.rotation = Quaternion.identity;

        //var input = _player.gameObject.AddComponent<KeyBoardInputSystem>();
        //_player.Init(input);
    }

    private void InitAi()
    {
        var skipPlace = _playerPlace == -1 ? _carPlaces.Count - 1 : _playerPlace;

        for (int i = 0; i < _carPlaces.Count - 1; i++)
        {
            if(skipPlace == i)
                continue;

            var enemy = Instantiate(_aiPrefabs[Random.Range(0, _aiPrefabs.Count)], _carPlaces[i].transform);
            enemy.transform.rotation = Quaternion.identity;

            _enemies.Add(enemy);
            _carPlaces[i].Init(enemy);
            //var input = enemy.gameObject.AddComponent<AITargetInputSystem>();
            //enemy.Init(input);
        }
    }
}
