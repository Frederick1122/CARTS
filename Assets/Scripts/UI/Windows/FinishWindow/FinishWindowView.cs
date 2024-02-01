using System;

namespace UI.Windows.Finish
{
    public class FinishWindowView : UIView
    {
        public event Action OnGoToMainMenuAction = delegate {  };

        public void GoToMainMenu()
        {
            OnGoToMainMenuAction?.Invoke();
        }
    }
}