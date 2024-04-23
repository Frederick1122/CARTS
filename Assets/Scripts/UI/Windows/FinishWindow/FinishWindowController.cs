using System;

namespace UI.Windows.Finish
{
    public class FinishWindowController : UIController
    {
        protected const string WIN_SOUND = "SFX/Win";
        protected const string LOSE_SOUND = "SFX/Lose";
        
        public event Action OnGoToMainMenuAction = delegate {  };

        protected FinishWindowModel _model;

        public void GoToMainMenu()
        {
            OnGoToMainMenuAction?.Invoke();
        }
        
        protected override UIModel GetViewData()
        {
            return _model;
        }
    }
}
