using ConfigScripts;
using Managers;
using Managers.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using UI.Elements;
using UI.Widgets.CurrencyWidget;
using UnityEngine;

namespace UI.Windows.Shop
{
    public class ShopWindowController : UIController
    {
        public event Action OnOpenLobby = delegate { };
        public event Action<CarData> OnCarInShopUpdate = delegate { };

        [SerializeField] private ShopItemController _shopItemController;

        [SerializeField] private ShopPreview _preview;
        
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
            base.Show();
            _cars = ((CarLibrary)CarLibrary.Instance).GetConfigsWithoutAI();
            SetCurrentCarIndex();

            _preview.StartPreview();

            UpdateShop();
            UIManager.Instance.GetWidgetUI().ShowWindow(typeof(CurrencyWidgetController), false);
        }

        public override void Hide()
        {
            base.Hide();
            _preview.StopPreview();
            UIManager.Instance.GetWidgetUI().HideWindow(typeof(CurrencyWidgetController));
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
            _preview.SpawnCar(data);
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
    }
}