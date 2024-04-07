using System;
using System.Collections.Generic;
using Base;
using GamePush;
using Lean.Localization;
using UnityEngine;

public class GPManager : Singleton<GPManager>
{
    private const string UI_PLAYER_LOCALIZATION = "UI_PLAYER";
    
    public event Action onUpdateLeaders;
    
    public event Action<LeaderboardFetchData> onUpdatePlayer;
    
    public event Action<bool> onRewardedClose;
    
    public bool IsAdsActive => _isAdsActive;
    private bool _isAdsActive = false;
    
    private List<LeaderboardFetchData> _leaders = new List<LeaderboardFetchData>();
    private List<LeaderboardFetchData> _leadersWithPlayer = new List<LeaderboardFetchData>();
    private LeaderboardFetchData _player;
    private Language _browserLanguage;
    private bool _isFetchLeaderboardActive = false;
    
    public List<LeaderboardFetchData> GetLeaders()
    {
        return _leadersWithPlayer;
    }

    public LeaderboardFetchData GetPlayer()
    {
        if (_player == null)
        {
            UpdatePlayer();
        }
        
        return _player;
    }

    public Language GetBrowserLanguage()
    {
        return _browserLanguage;
    }
    
    public void UpdateScore()
    {
        //_player.score = ScoreManager.Instance.GetBestScore();
        GP_Player.SetScore(_player.score);
        GP_Player.Sync();
        UpdateLeaders();
    }

    public void UpdateNickname(string newNickname)
    {
        _player.name = newNickname;
        GP_Player.SetName(_player.name);
        GP_Player.Sync();
        UpdateLeaders();
    }

    public void ShowRewardedAds()
    {
        _isAdsActive = true;
        GP_Ads.ShowRewarded();
    }

    public void FetchLeaderboard()
    {
        if(_isFetchLeaderboardActive)
            return;
        
        _isFetchLeaderboardActive = true;
        GP_Leaderboard.Fetch("scoreboard", withMe: WithMe.last, limit: 10);
    }
    
    protected override void Awake()
    {
        base.Awake();
        
        GP_Ads.OnRewardedStart += delegate { SetAdsState(true); };
        GP_Ads.OnRewardedClose += delegate { SetAdsState(false); };

        GP_Ads.OnFullscreenStart += delegate { SetAdsState(true); };
        GP_Ads.OnFullscreenClose += delegate { SetAdsState(false); };

        GP_Ads.OnPreloaderStart += delegate { SetAdsState(true); };
        GP_Ads.OnPreloaderClose += delegate { SetAdsState(false); };
        
        GP_Ads.ShowPreloader();
        GP_Ads.OnPreloaderClose += PreloaderClose;
        GP_Leaderboard.OnFetchSuccess += OnFetchSuccess;
        GP_Ads.OnRewardedClose += RewardedAdsClose;

        _browserLanguage = GP_Language.Current();
    }
    
    private void Start()
    {
        GP_Ads.ShowSticky();
        UpdatePlayer();
    }

    private void UpdatePlayer()
    {
        _player = new LeaderboardFetchData
        {
            id = GP_Player.GetID(),
            name = GP_Player.GetName() != "" ? GP_Player.GetName() : $"{LeanLocalization.GetTranslationText(UI_PLAYER_LOCALIZATION)}#{GP_Player.GetID()}",
            score = (int)GP_Player.GetScore()
        };
        
        onUpdatePlayer?.Invoke(_player);
    }
    
    private void OnDestroy()
    {
        GP_Leaderboard.OnFetchSuccess -= OnFetchSuccess;
        GP_Ads.OnRewardedClose -= RewardedAdsClose;
        GP_Ads.OnPreloaderClose -= PreloaderClose;
    }

    private void RewardedAdsClose(bool success)
    {
        onRewardedClose?.Invoke(success);
    }

    private void SetAdsState(bool isActive)
    {
        _isAdsActive = isActive;
        // var state = isActive ? AudioSourceCondition.Pause : AudioSourceCondition.Resume;
        //
        // BgAudioManager.Instance.SetBgMusic(state);
        // BgAudioManager.Instance.SetAmbienceSource(state);
        // // kostyl'
        // foreach (var source in FindObjectsOfType(typeof(AudioSource)))
        //     BgAudioManager.Instance.SetSource(state, (AudioSource) source);
    }

    private void PreloaderClose(bool success)
    {
        FetchLeaderboard();
    }
    
    private void OnFetchSuccess(string fetchTag, GP_Data data)
    {
        _isFetchLeaderboardActive = false;
        _leaders = data.GetList<LeaderboardFetchData>();
        UpdateLeaders();
    }
    
    private void UpdateLeaders()
    {
        if (_player == null)
            UpdatePlayer();
        
        _leadersWithPlayer = _leaders;
        
        if(_leadersWithPlayer.Count == 0)
        {
            _leadersWithPlayer.Add(_player);
        }
        else
        {
            for (var index = _leadersWithPlayer.Count - 1; index >= 0; index--)
            {
                if (_leadersWithPlayer[index].score == 0 || _leadersWithPlayer[index].id == _player.id)
                {
                    _leadersWithPlayer.RemoveAt(index);
                }
                else if (_leadersWithPlayer[index].name == "")
                {
                    _leadersWithPlayer[index].name =
                        $"{LeanLocalization.GetTranslationText(UI_PLAYER_LOCALIZATION)}#{_leadersWithPlayer[index].id}";
                }
            }
            
            for (var i = 0; i < _leadersWithPlayer.Count; i++)
            {
                if (_leadersWithPlayer[i].score < _player.score)
                {
                    _leadersWithPlayer.Insert(i, _player);
                    break;
                }

                if (i == _leadersWithPlayer.Count - 1)
                {
                    _leadersWithPlayer.Add(_player);
                    break;
                }
            }
        }
        
        onUpdateLeaders?.Invoke();
    }
}

[Serializable]
public class  LeaderboardFetchData
{
    public int id;
    public int score;
    public string name;
}