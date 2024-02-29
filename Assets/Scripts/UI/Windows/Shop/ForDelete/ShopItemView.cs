using ConfigScripts;
using Managers.Libraries;
using System;
using TMPro;
using UI.Elements;
using UI.Windows.Garage;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Windows.Shop
{
    public class ShopItemView : UIView
    {
        public event Action OnCarBuy = delegate { };

        [SerializeField] private Button _buyButton;
        [SerializeField] private CurrencyImage _currencyImage;
        [SerializeField] private TMP_Text _price;

        [Header("Characteristics")]
        [SerializeField] private CarCharacteristicShop _speed;
        [SerializeField] private CarCharacteristicShop _acceleration;
        [SerializeField] private CarCharacteristicShop _turnSpeed;

        public override void Init(UIModel uiModel)
        {
            //UpdateData((ShopItemModel)uiModel);
            _buyButton.onClick.AddListener(BuyCar);
        }

        private void OnDestroy() =>
            _buyButton.onClick.RemoveListener(BuyCar);

        public override void UpdateView(UIModel uiModel) =>
            UpdateData((ShopItemModel)uiModel);

        private void UpdateData(ShopItemModel model)
        {
            if(model.Purchased)
                _buyButton.gameObject.SetActive(false);
            else
            {
                _buyButton.gameObject.SetActive(true);
                _currencyImage.SetImage(model.Config.price.currencyType);
                _price.text = model.Config.price.value.ToString();
            }

            _speed.UpdateInfo(model.Config.maxSpeedLevels[0], model.Config.maxSpeedLevels[^1]); 
            _acceleration.UpdateInfo(model.Config.accelerationLevels[0], model.Config.accelerationLevels[^1]);
            _turnSpeed.UpdateInfo(model.Config.turnLevels[0], model.Config.turnLevels[^1]);
        }

        private void BuyCar()
        {
            _buyButton.gameObject.SetActive(false);
            OnCarBuy?.Invoke();
        }
    }

    public class ShopItemModel : UIModel
    {
        public CarConfig Config;

        public int Price = 0;
        public bool Purchased = false;

        public ShopItemModel(string configKey = "", bool purchased = false, int price = 0)
        {
            if (configKey == "")
                return;

            Config = CarLibrary.Instance.GetConfig(configKey);

            Price = price;
            Purchased = purchased;
        }
    }
}