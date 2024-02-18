using System;

namespace UI.Windows.Finish
{
    public class FinishWindowController : UIController
    {
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
