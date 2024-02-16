using System;
using TMPro;
using UnityEngine;

namespace UI.Windows.Finish
{
    public class FinishWindowView : UIView
    {
        [SerializeField] private TMP_Text _result;
        public event Action OnGoToMainMenuAction = delegate {  };

        public override void UpdateView(UIModel uiModel)
        {
            base.UpdateView(uiModel);

            var castData = (FinishWindowModel) uiModel;

            if (_result != null)
                _result.text = castData.result.ToString();
        }

        public void GoToMainMenu()
        {
            OnGoToMainMenuAction?.Invoke();
        }
    }
    
    public class FinishWindowModel : UIModel
    {
        public int result;
    }
}

