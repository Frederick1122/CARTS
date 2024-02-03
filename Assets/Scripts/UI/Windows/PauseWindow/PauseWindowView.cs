using System;

namespace UI.Windows.Pause
{
    public abstract class PauseWindowView : UIView
    {
        public event Action OnResume = delegate { };
        public event Action OnBackToLobby = delegate { };

        protected void Resume() =>
            OnResume?.Invoke();

        protected void BackToLobby() =>
            OnBackToLobby?.Invoke();
    }
}