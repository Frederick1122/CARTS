using Managers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Windows.Garage
{
    public class GarageWindowController : UIController
    {
        public event Action OnOpenLobby = delegate { };
        public event Action<CarData> OnCarInGarageUpdate = delegate { };

        [SerializeField] private GarageCarController _garageCarController;

        private CarData _currentCar => PlayerManager.Instance.GetCurrentCar();
        private IReadOnlyList<CarData> _cars;
        private int _currentCarIndex = 0;

        public override void Init()
        {
            _view.Init(GetViewData());

            _garageCarController.Init();
            _garageCarController.OnUpgrade += UpgradeCar;

            _cars = PlayerManager.Instance.GetPurchasedCars();

            GetView<GarageWindowView>().OnOpenLobby += RequestToOpenLobby;

            GetView<GarageWindowView>().OnNextCar += ChooseNextCar;
            GetView<GarageWindowView>().OnPrevCar += ChoosePrevCar;
        }

        private void OnDestroy()
        {
            _garageCarController.OnUpgrade -= UpgradeCar;

            GetView<GarageWindowView>().OnOpenLobby += RequestToOpenLobby;

            GetView<GarageWindowView>().OnNextCar -= ChooseNextCar;
            GetView<GarageWindowView>().OnPrevCar -= ChoosePrevCar;
        }

        public override void Show()
        {
            _cars = PlayerManager.Instance.GetPurchasedCars();
            SetCurrentCarIndex();
            UpdateGarageUI();

            base.Show();
            _garageCarController.Show();
        }

        public override void Hide()
        {
            base.Hide();
            _garageCarController.Hide();
        }

        protected override UIModel GetViewData()
        {
            return new GarageWindowModel();
        }

        private void UpdateGarageUI() =>
            _garageCarController.UpdateInfo(
                LobbyManager.Instance.Garage.SpawnedCarData, 
                LobbyManager.Instance.Garage.SpawnedCarPrefabData);

        private void UpgradeCar(ModificationType modification)
        {
            PlayerManager.Instance.UpdateModificationLevel(_cars[_currentCarIndex].configKey, modification);
            UpdateGarageUI();
        }

        private void EquipCar(string key) =>
            PlayerManager.Instance.SetCurrentCar(key);

        private void RequestToOpenLobby() =>
            OnOpenLobby?.Invoke();

        private void ChooseNextCar()
        {
            _currentCarIndex++;
            if (_currentCarIndex >= _cars.Count)
                _currentCarIndex = 0;

            EquipCar(_cars[_currentCarIndex].configKey);
            UpdateGarageUI();
            OnCarInGarageUpdate?.Invoke(_currentCar);
        }

        private void ChoosePrevCar()
        {
            _currentCarIndex--;
            if (_currentCarIndex < 0)
                _currentCarIndex = _cars.Count - 1;

            EquipCar(_currentCar.configKey);
            UpdateGarageUI();
            OnCarInGarageUpdate?.Invoke(_currentCar);
        }

        private void SetCurrentCarIndex()
        {
            for (int i = 0; i < _cars.Count; i++)
            {
                if (_currentCar.configKey == _cars[i].configKey)
                {
                    _currentCarIndex = i;
                    return;
                }
            }
        }
    }
}
