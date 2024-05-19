using System;
using System.Collections.Generic;
using Race.RaceManagers;
using UI.Windows;
using UI.Windows.Finish;
using UI.Windows.Pause;
using UI.Windows.Race.StartDelay;
using UnityEngine;

namespace UI
{
    public class RaceUI : MonoBehaviour
    {
        [SerializeField] private StartDelayController _startDelayController;
        [SerializeField] private List<RaceUIGroup> _raceUIGroups = new ();
        private Dictionary<RaceType, RaceUIGroup> _raceUIGroupsDict = new();

        public void Init()
        {
            foreach (var raceUIGroup in _raceUIGroups)
            {
                foreach (var raceLayout in raceUIGroup.raceLayouts)
                {
                    _raceUIGroupsDict.Add(raceLayout.raceType, raceUIGroup);
                    raceLayout.Init();                    
                }
                
                raceUIGroup.pauseWindowController.Init();
                raceUIGroup.finishWindowController.Init();
            }
            
            _startDelayController.Init();

            HideAll();
        }

        public StartDelayController GetStartDelayController()
        {
            return _startDelayController;
        }
        
        public void HideAll()
        {
            foreach (var raceUIGroup in _raceUIGroups)
            {
                foreach (var raceLayout in raceUIGroup.raceLayouts)
                    raceLayout.Hide();
                
                raceUIGroup.pauseWindowController.Hide();
                raceUIGroup.finishWindowController.Hide();
            }
            
            _startDelayController.Hide();
        }

        public PauseWindowController GetPauseWindowController(RaceType raceType)
        {
            return _raceUIGroupsDict[raceType].pauseWindowController;
        }
        
        public FinishWindowController GetFinishWindowController(RaceType raceType)
        {
            return _raceUIGroupsDict[raceType].finishWindowController;
        }
        
        public RaceLayoutController GetRaceLayout(RaceType raceType)
        {
            return _raceUIGroupsDict[raceType].raceLayouts.Find(x => x.raceType == raceType);
        }
    }
    
    [Serializable]
    public class RaceUIGroup
    {
        public List<RaceLayoutController> raceLayouts;
        public PauseWindowController pauseWindowController;
        public FinishWindowController finishWindowController;
    }
}
