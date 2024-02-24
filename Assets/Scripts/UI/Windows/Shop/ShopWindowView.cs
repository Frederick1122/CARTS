using System;
using TMPro;
using UI.Elements;
using UI.Windows.Garage;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.Shop
{
    public class ShopWindowView : UIView 
    {
        public event Action OnNextCar = delegate { };
        public event Action OnPrevCar = delegate { };
        public event Action OnOpenLobby = delegate { };

        [SerializeField] private TMP_Text _name;
        [SerializeField] protected Button _backButton;

        [Header("Scroll")]
        [SerializeField] protected Button _nextCarButton;
        [SerializeField] protected Button _prevCarButton;

        private readonly ShopWindowModel _model = new();

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

        public void UpdateCarName(string name)
        {
            _model.CarName = name;
            UpdateView(_model);
        }

        public override void UpdateView(UIModel uiModel) => _name.text = _model.CarName;
    }

    public class ShopWindowModel : UIModel 
    {
        public string CarName = "";
    }
}