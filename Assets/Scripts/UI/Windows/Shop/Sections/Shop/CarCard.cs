using ConfigScripts;
using System;
using TMPro;
using UI.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.Shop.Sections.SaleShop
{
    public class CarCard : MonoBehaviour
    {
        public event Action<CarConfig> OnCarBuy = delegate(CarConfig config) { };

        public CarConfig CarConfig => _carConfig;

        [SerializeField] private Button _buyButton;
        [SerializeField] private TMP_Text _buyButtonText;
        [SerializeField] private Image _carIcon;
        [SerializeField] private TMP_Text _carName;

        private CarConfig _carConfig;

        public void Init(CarConfig config)
        {
            UpdateCar(config, BuyButtonCondition.CanBuy);
            _buyButton.onClick.AddListener(RequestForCarBuy);
        }

        private void OnDestroy()
        {
            _buyButton.onClick.RemoveListener(RequestForCarBuy);
        }

        public void UpdateCar(CarConfig config, BuyButtonCondition buyButtonCondition)
        {
            _carConfig = config;
            _carIcon.sprite = config.CarIcon;
            _carName.text = config.configName;
            ChangeBuyButtonCondition(buyButtonCondition);
        }

        private void ChangeBuyButtonCondition(BuyButtonCondition buyButtonCondition)
        {
            switch (buyButtonCondition)
            {
                case BuyButtonCondition.CanBuy:
                    _buyButton.interactable = true;
                    _buyButton.MakeChosenVisual();
                    _buyButtonText.text = _carConfig.price.Value.ToString();
                    break;
                case BuyButtonCondition.NotEnoughMoney:
                    _buyButton.interactable = false;
                    _buyButton.MakeUnChosenVisual();
                    _buyButtonText.text = "Not enough money";
                    break;
                case BuyButtonCondition.Purchased:
                    _buyButton.interactable = false;
                    _buyButton.MakeUnChosenVisual();
                    _buyButtonText.text = "Purchased";
                    break;
            }
        }

        private void RequestForCarBuy()
        { 
            OnCarBuy?.Invoke(_carConfig); 
        }
        public enum BuyButtonCondition
        {
            CanBuy = 0,
            NotEnoughMoney = 1,
            Purchased = 2,
        };
    }
}
