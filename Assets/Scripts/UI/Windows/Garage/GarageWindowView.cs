using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.Garage
{
    public class GarageWindowView : UIView
    {
        public event Action OnNextCar = delegate { };
        public event Action OnPrevCar = delegate { };

        public event Action OnOpenLobby = delegate { };

        [SerializeField] protected Button _backButton;

        [Header("Scroll")]
        [SerializeField] protected Button _nextCarButton;
        [SerializeField] protected Button _prevCarButton;

        public override void Init(UIModel uiModel)
        {
            _backButton.onClick.AddListener(OpenLobby);

            _nextCarButton.onClick.AddListener(NextCar);
            _prevCarButton.onClick.AddListener(PrevCar);
        }

        private void OnDestroy()
        {
            _backButton.onClick.RemoveListener(OpenLobby);

            _nextCarButton.onClick.RemoveListener(NextCar);
            _prevCarButton.onClick.RemoveListener(PrevCar);
        }

        public void OpenLobby() =>
            OnOpenLobby?.Invoke();

        public void NextCar() =>
            OnNextCar?.Invoke();

        public void PrevCar() =>
            OnPrevCar?.Invoke();
    }

    public class GarageWindowModel : UIModel { }
}
