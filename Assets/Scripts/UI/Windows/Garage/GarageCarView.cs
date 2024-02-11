using Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UI.Elements;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.Garage
{
    public class GarageCarView : UIView
    {
        public event Action<ModificationType> OnUpgrade = delegate { };

        [Header("Characteristics")]
        [SerializeField] private CarCharacteristicGarage _speed;
        [SerializeField] private CarCharacteristicGarage _acceleration;
        [SerializeField] private CarCharacteristicGarage _turnSpeed;

        public override void Init(UIModel uiModel)
        {
            UpdateData((GarageCarModel)uiModel);

            _speed.Init();
            _speed.OnCharacteristicUpgrade += Upgrade;

            _acceleration.Init();
            _acceleration.OnCharacteristicUpgrade += Upgrade;

            _turnSpeed.Init();
            _turnSpeed.OnCharacteristicUpgrade += Upgrade;
        }

        private void OnDestroy()
        {
            _speed.OnCharacteristicUpgrade -= Upgrade;
            _acceleration.OnCharacteristicUpgrade -= Upgrade;
            _turnSpeed.OnCharacteristicUpgrade -= Upgrade;
        }

        private void Upgrade(ModificationType modification) =>
            OnUpgrade?.Invoke(modification);

        public override void UpdateView(UIModel uiModel) =>
            UpdateData((GarageCarModel)uiModel);

        private void UpdateData(GarageCarModel model)
        {
            _speed.UpdateInfo(model.speedLvl, 0);
            _acceleration.UpdateInfo(model.accelerationLvl, 0);
            _turnSpeed.UpdateInfo(model.turnSpeedLvl, 0);
        }
    }

    public class GarageCarModel : UIModel
    {
        public int speedLvl = 0;
        public int accelerationLvl = 0;
        public int turnSpeedLvl = 0;
    }
}
