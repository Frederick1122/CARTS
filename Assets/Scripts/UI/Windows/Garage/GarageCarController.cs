using Cars;
using ConfigScripts;
using Managers;
using Managers.Libraries;
using System;
using UnityEngine;

namespace UI.Windows.Garage
{
    public class GarageCarController : UIController
    {
        public event Action<ModificationType> OnUpgrade = delegate { };

        private readonly GarageCarModel _model = new();

        public override void Init()
        {
            GetView<GarageCarView>().OnUpgrade += Upgrade;
            base.Init();
        }

        private void OnDestroy() =>
            GetView<GarageCarView>().OnUpgrade -= Upgrade;

        public void UpdateInfo(CarData data, CarConfig spawnedCar)
        {
            _model.SpeedLvl = data.maxSpeedLevel + 1;
            _model.AccelerationLvl = data.accelerationLevel + 1;
            _model.TurnSpeedLvl = data.turnLevel + 1;

            _model.CarConfig = spawnedCar;

            var carConfig = CarLibrary.Instance.GetConfig(data.configKey);
            var speedCost = _model.SpeedLvl >= carConfig.maxSpeedLevels.Count ? -1 : carConfig.maxSpeedLevels[data.maxSpeedLevel + 1].Price;
            var turnCost = _model.TurnSpeedLvl >= carConfig.turnLevels.Count ? -1 : carConfig.turnLevels[data.turnLevel + 1].Price;
            var accelerationCost = _model.AccelerationLvl >= carConfig.accelerationLevels.Count ? -1 : carConfig.accelerationLevels[data.accelerationLevel + 1].Price;

            _model.SpeedCost = speedCost;
            _model.TurnCost = turnCost;
            _model.AccelerationCost = accelerationCost;

            UpdateView();
        }

        protected override UIModel GetViewData() { return _model; }

        private void Upgrade(ModificationType modification) =>
            OnUpgrade?.Invoke(modification);
    }
}
