using ConfigScripts;
using Swiper;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Windows.Shop.Sections.SaleShop
{
    public class SaleShopWindowView : UIView
    {
        public event Action<CarConfig> OnCarBuy = delegate { };

        [SerializeField] private CarCard[] _carCards = new CarCard[4];

        private readonly Dictionary<Rarity, CarCard> _carCardsByRarity = new();

        public override void Init(UIModel uiModel)
        {
            var castModel = (SaleShopWindowModel)uiModel;

            _carCardsByRarity.Add(Rarity.Common, _carCards[0]);
            _carCardsByRarity.Add(Rarity.Uncommon, _carCards[1]);
            _carCardsByRarity.Add(Rarity.Rare, _carCards[2]);
            _carCardsByRarity.Add(Rarity.Legendary, _carCards[3]);

            foreach (var dailyCarPair in castModel.DailyCars)
            {
                _carCardsByRarity[dailyCarPair.Key].Init(dailyCarPair.Value);
                _carCardsByRarity[dailyCarPair.Key].OnCarBuy += RequestForBuyCar;
            }
        }

        private void OnDestroy()
        {
            foreach (var card in _carCardsByRarity.Values)
                card.OnCarBuy -= RequestForBuyCar;
        }

        public override void UpdateView(UIModel uiModel)
        {
            var castModel = (SaleShopWindowModel)uiModel;
            foreach (var dailyCarPair in castModel.DailyCars)
            {
                var config = dailyCarPair.Value;
                var buttonCodition = castModel.CarBuyButtonConditions[dailyCarPair.Key];
                _carCardsByRarity[dailyCarPair.Key].UpdateCar(config, buttonCodition);
            }
        }

        private void RequestForBuyCar(CarConfig config) => OnCarBuy?.Invoke(config);
    }

    public class SaleShopWindowModel : UIModel 
    {
        public IReadOnlyDictionary<Rarity, CarConfig> DailyCars => _dailyCars;
        public IReadOnlyDictionary<Rarity, CarCard.BuyButtonCondition> CarBuyButtonConditions => _carBuyButtonConditions;

        private readonly Dictionary<Rarity, CarConfig> _dailyCars = new();
        private readonly Dictionary<Rarity, CarCard.BuyButtonCondition> _carBuyButtonConditions = new();

        public SaleShopWindowModel(Dictionary<Rarity, CarConfig> dailyCars,
            Dictionary<Rarity, CarCard.BuyButtonCondition> carBuyButtonConditions)
        {
            _dailyCars = dailyCars;
            _carBuyButtonConditions = carBuyButtonConditions;
        }
    }
}