#if PHOTON_UNITY_NETWORKING
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carListPUN_MCR : MonoBehaviour
{
    public static carListPUN_MCR    instance;
    public bool                     SeeInspector = false;

    public bool                     SeeCarList = true;
    public bool                     SeeTrackList = true;

    [System.Serializable]
    public class _PlayerList
    {
        public string _NickName = "";
        public int _Score = 0;
    }

    public List<_PlayerList>        _Players = new List<_PlayerList>();
    public InventoryCar             inventoryCar;
    public inventoryOnlineTracks    inventoryOnlineTracks;
    public List<int>                scoreTable = new List<int>();
    public int                      selectedCar = 0;
    public string                   playerNameThisComputer = "";


    public int maxPlayerByRoom = 4;
    public int maxCPU = 4;

    public int HowManyCarsInCurrentRace = 0;

    public void Awake()
    {
        if (instance == null)
            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);


        DontDestroyOnLoad(this.gameObject);
    }


    public void changeSelectedCar(int _playerID)
    {
        selectedCar++;
        selectedCar %= inventoryCar.inventoryItem[_playerID].Cars.Count;
    }

    public void changeSelectedCarPrevious(int _playerID)
    {
        selectedCar--;
        if (selectedCar < 0)
            selectedCar = inventoryCar.inventoryItem[_playerID].Cars.Count-1;

        //selectedCar %= inventoryCar.inventoryItem[_playerID].Cars.Count;
    }


    public void MCR_ResetPlayersScores()
    {
        _Players = new List<_PlayerList>();
    }

    public void MCR_AddToPlayerScore(string _NickName,int _Points)
    {
        bool b_Exist = false;
        for(var i = 0;i< _Players.Count; i++)
        {
            if (_NickName == _Players[i]._NickName) {
                _Players[i]._Score += _Points;
                b_Exist = true;
                break;
            }
        }

        if (!b_Exist)
        {
            _Players.Add(new _PlayerList());
            _Players[_Players.Count - 1]._NickName = _NickName;
            _Players[_Players.Count - 1]._Score = _Points;
        }

    }
}
#endif
