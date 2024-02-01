using System;
using System.Collections.Generic;
using System.Linq;
using UI.Windows;
using UI.Windows.Finish;
using UI.Windows.Pause;
using UnityEngine;

namespace UI
{
    public class RaceUI : MonoBehaviour
    {
        [SerializeField] private List<RaceUIGroup> _raceUIGroups = new ();
        private Dictionary<Type, RaceUIGroup> _raceUIGroupsDict = new();

        public void Init()
        {
            foreach (var raceUIGroup in _raceUIGroups)
            {
                foreach (var raceLayout in raceUIGroup.raceLayouts)
                {
                    _raceUIGroupsDict.Add(raceLayout.GetType(), raceUIGroup);
                    raceLayout.Init();                    
                }
                
                raceUIGroup.pauseWindowController.Init();
                raceUIGroup.finishWindowController.Init();
            }
            
            
            HideAll();
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
        }

        public PauseWindowController GetPauseWindowController<RaceLayout>()
        {
            return _raceUIGroupsDict[typeof(RaceLayout)].pauseWindowController;
        }
        
        
        public FinishWindowController GetFinishWindowController<RaceLayout>()
        {
            return _raceUIGroupsDict[typeof(RaceLayout)].finishWindowController;
        }
        
        
        public RaceLayout GetRaceLayout<RaceLayout>()
        {
            return _raceUIGroupsDict[typeof(RaceLayout)].raceLayouts.OfType<RaceLayout>().FirstOrDefault();
        }
    }
    
    [Serializable]
    public class RaceUIGroup
    {
        public List<RaceLayout> raceLayouts;
        public PauseWindowController pauseWindowController;
        public FinishWindowController finishWindowController;
    }
}
