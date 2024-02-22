using Cars;
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

        public void UpdateInfo(CarData data, CarPrefabData spawnedCar)
        {
            _model.SpeedLvl = data.maxSpeedLevel;
            _model.AccelerationLvl = data.accelerationLevel;
            _model.TurnSpeedLvl = data.turnLevel;

            _model.CarPrefabData = spawnedCar;

            UpdateView();
        }

        protected override UIModel GetViewData() { return _model; }

        private void Upgrade(ModificationType modification) =>
            OnUpgrade?.Invoke(modification);
    }
}
