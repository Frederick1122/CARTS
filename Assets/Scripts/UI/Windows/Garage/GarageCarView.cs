using Cars;
using ConfigScripts;
using Managers;
using System;
using System.Data;
using UI.Elements;
using UnityEngine;

namespace UI.Windows.Garage
{
    public class GarageCarView : UIView
    {
        public event Action<ModificationType> OnUpgrade = delegate { };

        [SerializeField] private Canvas _uiCanvas;
        [SerializeField] private Camera _uiCamera;

        [Header("Characteristics")]
        [SerializeField] private FloatingUICarCharacteristic _speed;
        [SerializeField] private FloatingUICarCharacteristic _acceleration;
        [SerializeField] private FloatingUICarCharacteristic _turnSpeed;

        public override void Init(UIModel uiModel)
        {
            var castModel = (GarageCarModel)uiModel;
            _speed.Init();
            _acceleration.Init();
            _turnSpeed.Init();

            UpdateData(castModel);

            _speed.OnCharacteristicUpgrade += Upgrade;
            _acceleration.OnCharacteristicUpgrade += Upgrade;
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
            if (model.CarConfig == null)
                return;

            _speed.UpdateInfo(model.SpeedLvl, model.CarConfig.maxSpeedLevels.Count, 
                model.SpeedCost);
            _acceleration.UpdateInfo(model.AccelerationLvl, model.CarConfig.accelerationLevels.Count, 
                model.AccelerationCost);
            _turnSpeed.UpdateInfo(model.TurnSpeedLvl, model.CarConfig.turnLevels.Count, 
                model.TurnCost);
        }
    }

    public class GarageCarModel : UIModel
    {
        public CarConfig CarConfig { get; set; }

        public int SpeedLvl { get; set; } = 0;
        public int SpeedCost { get; set; } = -1;

        public int AccelerationLvl { get; set; } = 0;
        public int AccelerationCost { get; set; } = -1;

        public int TurnSpeedLvl { get; set; } = 0;
        public int TurnCost { get; set; } = -1;
    }
}
