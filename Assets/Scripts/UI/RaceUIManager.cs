using Base;
using System;
using System.Collections.Generic;
using UI.Windows.Finish;
using UI.Windows.Pause;
using UI.Windows.Race;
using UnityEngine;

namespace UI
{
    public class RaceUIManager : Singleton<RaceUIManager>
    {
        public RaceWindowController RaceUI { get; private set; }
        public PauseWindowController PauseUI { get; private set; }
        public FinishWindowController FinishUI { get; private set; }

        [SerializeField] private List<RaceUIGroup> _raceUIGroups = new();

        private readonly Dictionary<Type, RaceUIGroup> _raceUIGroupsByRaceWindow = new();

        public void Init()
        {
            foreach (var raceUIGroup in _raceUIGroups)
            {
                foreach (var raceLayout in raceUIGroup.raceWindows)
                {
                    _raceUIGroupsByRaceWindow.Add(raceLayout.GetType(), raceUIGroup);
                }
            }
        }

        public void SetUpUI(Type type)
        {
            var group = _raceUIGroupsByRaceWindow[type];

            RaceUI = group.raceWindows.Find(win => win.GetType() == type);
            PauseUI = group.pauseWindow;
            FinishUI = group.finishWindow;
        }

        [Serializable]
        public class RaceUIGroup
        {
            public List<RaceWindowController> raceWindows;
            public PauseWindowController pauseWindow;
            public FinishWindowController finishWindow;
        }
    }
}

