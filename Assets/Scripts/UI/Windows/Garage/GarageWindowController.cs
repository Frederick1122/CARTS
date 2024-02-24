using Managers;
using Managers.Libraries;
using Swiper;
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
        [SerializeField] private SwiperController _carSwiper;

        private CarData _currentCar => PlayerManager.Instance.GetCurrentCar();
        private IReadOnlyList<CarData> _cars;
        private string _currentCarKey = "";

        public override void Init()
        {
            //SetUpPurchasedCarSwiper();
            _currentCarKey = _currentCar.configKey;

            _view.Init(GetViewData());

            _garageCarController.Init();
            _garageCarController.OnUpgrade += UpgradeCar;

            GetView<GarageWindowView>().OnOpenLobby += RequestToOpenLobby;

            _carSwiper.Init();
            _carSwiper.OnTabClick += ChooseCarFromPurchased;
        }

        private void OnDestroy()
        {
            _garageCarController.OnUpgrade -= UpgradeCar;

            _carSwiper.OnTabClick -= ChooseCarFromPurchased;

            GetView<GarageWindowView>().OnOpenLobby += RequestToOpenLobby;
        }

        public override void Show()
        {
            SetUpPurchasedCarSwiper();
            UpdateGarageUI();

            base.Show();
            _garageCarController.Show();
        }

        public override void Hide()
        {
            base.Hide();
            _garageCarController.Hide();
        }

        protected override UIModel GetViewData() { return new GarageWindowModel(); }

        private void RequestToOpenLobby() => OnOpenLobby?.Invoke();

        private void UpdateGarageUI()
        {
            var garage = LobbyManager.Instance.Garage;
            _garageCarController.UpdateInfo(garage.SpawnedCarData, garage.SpawnedCarPrefabData);
        }

        private void SetUpPurchasedCarSwiper()
        {
            _cars = PlayerManager.Instance.GetPurchasedCars();
            _currentCarKey = _currentCar.configKey;

            _carSwiper.Clear();
            foreach (var car in _cars)
            {
                var config = CarLibrary.Instance.GetConfig(car.configKey);
                var data = new SwiperData(car.configKey, config.CarIcon, config.configName);
                _carSwiper.AddItems(data);
            }
        }

        private void ChooseCarFromPurchased(SwiperData data)
        {
            EquipCar(data.Key);
            OnCarInGarageUpdate?.Invoke(_currentCar);
            UpdateGarageUI();
        }

        private void EquipCar(string key) =>
            PlayerManager.Instance.SetCurrentCar(key);

        private void UpgradeCar(ModificationType modification)
        {
            PlayerManager.Instance.UpdateModificationLevel(_currentCarKey, modification);
            UpdateGarageUI();
        }

        //private void ChooseNextCar()
        //{
        //    _currentCarIndex++;
        //    if (_currentCarIndex >= _cars.Count)
        //        _currentCarIndex = 0;

        //    EquipCar(_cars[_currentCarIndex].configKey);
        //    UpdateGarageUI();
        //    OnCarInGarageUpdate?.Invoke(_currentCar);
        //}

        //private void ChoosePrevCar()
        //{
        //    _currentCarIndex--;
        //    if (_currentCarIndex < 0)
        //        _currentCarIndex = _cars.Count - 1;

        //    EquipCar(_currentCar.configKey);
        //    UpdateGarageUI();
        //    OnCarInGarageUpdate?.Invoke(_currentCar);
        //}

        //private void SetCurrentCarIndex()
        //{
        //    for (int i = 0; i < _cars.Count; i++)
        //    {
        //        if (_currentCar.configKey == _cars[i].configKey)
        //        {
        //            _currentCarIndex = i;
        //            return;
        //        }
        //    }
        //}
    }
}
