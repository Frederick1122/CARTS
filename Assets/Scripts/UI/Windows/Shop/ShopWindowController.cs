﻿using Managers;
using Managers.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using UI.Elements;
using UnityEngine;

namespace UI.Windows.Shop
{
    public class ShopWindowController : UIController
    {
        public event Action OpenLobbyAction;
        public event Action OnNewCarBuy;

        [SerializeField] private ShopCarCustomScroll _shopCarCustomScroll;

        private readonly List<ShopCarModel> _carModels = new();

        public override void Init()
        {
            GetView<ShopWindowView>().OpenLobbyAction += OpenLobby;
            _shopCarCustomScroll.OnSelectCarAction += SelectedNewCar;
            InitAllShopCars();
            base.Init();
        }

        private void OnDestroy()
        {
            if (_view != null)
                GetView<ShopWindowView>().OpenLobbyAction -= OpenLobby;


            if (_shopCarCustomScroll != null)
                _shopCarCustomScroll.OnSelectCarAction -= SelectedNewCar;
        }

        public override void UpdateView() =>
            UpdateAllShopCars();

        protected override UIModel GetViewData()
        {
            return new ShopWindowModel();
        }

        private void OpenLobby() =>
            OpenLobbyAction?.Invoke();

        private void SelectedNewCar(ShopCarModel uiModel)
        {
            var currentCarConfig = CarLibrary.Instance.GetConfig(PlayerManager.Instance.GetCurrentCar().configKey);

            if (uiModel.configKey == currentCarConfig.configName)
                return;

            PlayerManager.Instance.AddPurchasedCar(uiModel.configKey);
            PlayerManager.Instance.SetCurrentCar(uiModel.configKey);
            OnNewCarBuy?.Invoke();
            UpdateAllShopCars();
        }

        private void InitAllShopCars()
        {
            var carConfigs = CarLibrary.Instance.GetAllConfigs().Values.ToList();
            var currentCar = PlayerManager.Instance.GetCurrentCar();
            foreach (var carConfig in carConfigs)
            {
                if (carConfig.isOnlyForAi)
                    continue;

                var isSelectedCar = carConfig.configKey == currentCar.configKey;
                _carModels.Add(new ShopCarModel(carConfig.configKey, isSelectedCar));
            }

            _shopCarCustomScroll.AddRange(_carModels);
        }

        private void UpdateAllShopCars()
        {
            var currentCarConfig = CarLibrary.Instance.GetConfig(PlayerManager.Instance.GetCurrentCar().configKey);

            foreach (var carModel in _carModels)
                carModel.isSelectedCar = carModel.configKey == currentCarConfig.configKey;

            _shopCarCustomScroll.HideAll();
            _shopCarCustomScroll.AddRange(_carModels);
        }
    }
}