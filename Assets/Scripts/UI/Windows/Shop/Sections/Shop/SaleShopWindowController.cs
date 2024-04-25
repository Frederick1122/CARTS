using ConfigScripts;
using Managers;
using Managers.Libraries;
using System;
using System.Collections.Generic;

namespace UI.Windows.Shop.Sections.SaleShop
{
    public class SaleShopWindowController : UIController
    {
        private const string BUY_SOUND = "SFX/UI/Buy";
        
        private readonly Dictionary<Rarity, List<CarConfig>> _carsByRarity = new();

        public override void Init()
        {
            InitDictionary();
            base.Init();
            GetView<SaleShopWindowView>().OnCarBuy += BuyCar;
        }

        private void OnDestroy()
        {
            GetView<SaleShopWindowView>().OnCarBuy -= BuyCar;
        }

        public override void Show()
        {
            UpdateView();
            base.Show();
        }

        protected override UIModel GetViewData()
        {
            var commonCar = CalculateDailyCar(_carsByRarity[Rarity.Common]);
            var uncommonCar = CalculateDailyCar(_carsByRarity[Rarity.Uncommon]);
            var rareCar = CalculateDailyCar(_carsByRarity[Rarity.Rare]);
            var legendaryCar = CalculateDailyCar(_carsByRarity[Rarity.Legendary]);

            var dailyCars = new Dictionary<Rarity, CarConfig>()
            {
                { Rarity.Common, commonCar},
                { Rarity.Uncommon, uncommonCar},
                { Rarity.Rare, rareCar},
                { Rarity.Legendary, legendaryCar}
            };
            var buttonsCondition = new Dictionary<Rarity, CarCard.BuyButtonCondition>()
            {
                { Rarity.Common, CalculateCarBuyButtonCondition(commonCar) },
                { Rarity.Uncommon, CalculateCarBuyButtonCondition(uncommonCar) },
                { Rarity.Rare, CalculateCarBuyButtonCondition(rareCar) },
                { Rarity.Legendary, CalculateCarBuyButtonCondition(legendaryCar) }
            };

            var model = new SaleShopWindowModel(dailyCars, buttonsCondition);
            return model;
        }

        private void BuyCar(CarConfig config)
        {
            if (PlayerManager.Instance.TryToPurchaseCar(config.configKey))
            {
                SoundManager.Instance.PlayOneShot(BUY_SOUND);
            }
            UpdateView();
        }

        private CarCard.BuyButtonCondition CalculateCarBuyButtonCondition(CarConfig dailyCar)
        {
            if (PlayerManager.Instance.TryGetPurchasedCarData(dailyCar.configKey, out _))
                return CarCard.BuyButtonCondition.Purchased;
            if (PlayerManager.Instance.IsEnoughMoney(dailyCar.price))
                return CarCard.BuyButtonCondition.CanBuy;
            return CarCard.BuyButtonCondition.NotEnoughMoney;
        }

        private void InitDictionary()
        {
            var carLibrary = (CarLibrary)CarLibrary.Instance;

            _carsByRarity.Add(Rarity.Default, new());
            _carsByRarity.Add(Rarity.Common, new());
            _carsByRarity.Add(Rarity.Uncommon, new());
            _carsByRarity.Add(Rarity.Rare, new());
            _carsByRarity.Add(Rarity.Legendary, new());

            _carsByRarity[Rarity.Common].AddRange(carLibrary.GetAllConfigsByRarity(Rarity.Common));
            _carsByRarity[Rarity.Uncommon].AddRange(carLibrary.GetAllConfigsByRarity(Rarity.Uncommon));
            _carsByRarity[Rarity.Rare].AddRange(carLibrary.GetAllConfigsByRarity(Rarity.Rare));
            _carsByRarity[Rarity.Legendary].AddRange(carLibrary.GetAllConfigsByRarity(Rarity.Legendary));
        }

        private CarConfig CalculateDailyCar(List<CarConfig> configs)
        {
            var currentDay = DateTime.Now.Day;
            return configs[currentDay % configs.Count];
        }
    }
}
