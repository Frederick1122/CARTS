using System;
using UnityEngine;

namespace UI.Windows.Pause
{
    public abstract class PauseWindowController : UIController
    {
        public event Action OnBackToLobby = delegate { };
        public event Action OnResume = delegate { };

        protected readonly PauseWindowModel _model = new();

        protected override UIModel GetViewData()
        {
            return _model;
        }

        protected void BackToLobby() =>
            OnBackToLobby?.Invoke();

        protected void Resume() => 
            OnResume?.Invoke();
    }

    public class PauseWindowModel : UIModel { }
}
