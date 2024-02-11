using ConfigScripts;
using Managers;
using Managers.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI.Windows.Shop
{
    public class ShopWindowController : UIController
    {
        public event Action OnOpenLobby = delegate { };
        public event Action<CarData> OnCarInShopUpdate = delegate { };

        [SerializeField] private ShopItemController _shopItemController;
        [SerializeField] private Transform _carPreviewPlace;

        private CarData _currentCarData => PlayerManager.Instance.GetCurrentCar();
        private string _currentCarKey => _cars[_currentCarIndex].configKey;
        private IReadOnlyList<CarConfig> _cars;
        private int _currentCarIndex = 0;

        public override void Init()
        {
            _view.Init(GetViewData());
            _shopItemController.Init();

            _cars = ((CarLibrary)CarLibrary.Instance).GetConfigsWithoutAI();

            _shopItemController.OnCarBuy += BuyCar;

            GetView<ShopWindowView>().OnOpenLobby += RequestToOpenLobby;

            GetView<ShopWindowView>().OnNextCar += ChooseNextCar;
            GetView<ShopWindowView>().OnPrevCar += ChoosePrevCar;
        }

        private void OnDestroy()
        {
            _shopItemController.OnCarBuy -= BuyCar;

            GetView<ShopWindowView>().OnOpenLobby += RequestToOpenLobby;

            GetView<ShopWindowView>().OnNextCar -= ChooseNextCar;
            GetView<ShopWindowView>().OnPrevCar -= ChoosePrevCar;
        }

        public override void Show()
        {
            _cars = ((CarLibrary)CarLibrary.Instance).GetConfigsWithoutAI();
            SetCurrentCarIndex();

            base.Show();
            _shopItemController.Show();

            UpdateShop();
        }

        public override void Hide()
        {
            base.Hide();
            _shopItemController.Hide();
        }

        protected override UIModel GetViewData()
        {
            return new ShopWindowModel();
        }

        private void UpdateShop()
        {
            var data = new CarData(_currentCarKey);

            _shopItemController.UpdateInfo(data);
            GetView<ShopWindowView>().UpdateCarName(_cars[_currentCarIndex].configName);
            UpdatePreview(data);
        }

        private void BuyCar()
        {
            PlayerManager.Instance.AddPurchasedCar(_currentCarKey);
            PlayerManager.Instance.SetCurrentCar(_currentCarKey);
        }

        private void RequestToOpenLobby() =>
           OnOpenLobby?.Invoke();

        private void ChooseNextCar()
        {
            _currentCarIndex++;
            if (_currentCarIndex >= _cars.Count)
                _currentCarIndex = 0;

            UpdateShop();
            OnCarInShopUpdate?.Invoke(new CarData(_currentCarKey));
        }

        private void ChoosePrevCar()
        {
            _currentCarIndex--;
            if (_currentCarIndex < 0)
                _currentCarIndex = _cars.Count - 1;

            UpdateShop(); 
            OnCarInShopUpdate?.Invoke(new CarData(_currentCarKey));
        }

        private void SetCurrentCarIndex()
        {
            for (int i = 0; i < _cars.Count; i++)
            {
                if (_currentCarData.configKey == _cars[i].configKey)
                {
                    _currentCarIndex = i;
                    return;
                }
            }
        }

        public void UpdatePreview(CarData data)
        {
            for (int i = 0; i < _carPreviewPlace.childCount; i++)
                Destroy(_carPreviewPlace.GetChild(i).gameObject);

            var carPref = CarLibrary.Instance.GetConfig(data.configKey).prefab;

            var car = Instantiate(carPref, _carPreviewPlace).gameObject;
            Destroy(car.GetComponent<Rigidbody>());

            SetGameLayerRecursive(car, 10);
        }

        private void SetGameLayerRecursive(GameObject gameObject, int layer)
        {
            gameObject.layer = layer;
            foreach (Transform child in gameObject.transform)
            {
                SetGameLayerRecursive(child.gameObject, layer);
            }
        }
    }
}