using Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Windows.Garage
{
    public class GarageCarController : UIController
    {
        public event Action<ModificationType> OnUpgrade = delegate { };
        public event Action OnEquipCar = delegate { };

        private GarageCarModel _model = new();

        public override void Init()
        {
            GetView<GarageCarView>().OnUpgrade += Upgrade;
            GetView<GarageCarView>().OnEquip += EquipCar;

            _view.Init(_model);
        }

        private void OnDestroy()
        {
            GetView<GarageCarView>().OnUpgrade -= Upgrade;
            GetView<GarageCarView>().OnEquip -= EquipCar;
        }

        public override void UpdateView() =>
            _view.UpdateView(_model);

        public void UpdateInfo(CarData data, bool isEquipped)
        {
            _model.speedLvl = data.maxSpeedLevel;
            _model.accelerationLvl = data.accelerationLevel;
            _model.turnSpeedLvl = data.turnLevel;

            _model.isEquipped = isEquipped;

            UpdateView();
        }

        protected override UIModel GetViewData()
        {
            return _model;
        }

        private void Upgrade(ModificationType modification) =>
            OnUpgrade?.Invoke(modification);

        private void EquipCar() =>
            OnEquipCar?.Invoke();
    }
}
