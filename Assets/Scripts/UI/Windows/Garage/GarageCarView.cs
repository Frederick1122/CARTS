using Cars;
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
            UpdateData((GarageCarModel)uiModel);

            _speed.Init(_uiCamera, _uiCanvas);
            _speed.OnCharacteristicUpgrade += Upgrade;
            _speed.DisableLine();

            _acceleration.Init(_uiCamera, _uiCanvas);
            _acceleration.OnCharacteristicUpgrade += Upgrade;
            _acceleration.DisableLine();

            _turnSpeed.Init(_uiCamera, _uiCanvas);
            _turnSpeed.OnCharacteristicUpgrade += Upgrade;
            _turnSpeed.DisableLine();
        }

        private void OnDestroy()
        {
            _speed.OnCharacteristicUpgrade -= Upgrade;
            _acceleration.OnCharacteristicUpgrade -= Upgrade;
            _turnSpeed.OnCharacteristicUpgrade -= Upgrade;
        }

        public override void Hide()
        {
            _speed.DisableLine();
            _turnSpeed.DisableLine();
            _acceleration.DisableLine();
            base.Hide();
        }

        private void Upgrade(ModificationType modification) =>
            OnUpgrade?.Invoke(modification);

        public override void UpdateView(UIModel uiModel) =>
            UpdateData((GarageCarModel)uiModel);

        private void UpdateData(GarageCarModel model)
        {
            _speed.UpdateInfo(model.SpeedLvl, model.SpeedCost);
            _acceleration.UpdateInfo(model.AccelerationLvl, model.AccelerationCost);
            _turnSpeed.UpdateInfo(model.TurnSpeedLvl, model.TurnCost) ;

            var data = model.CarPrefabData;
            if (data == null)
                return;
            DrawLine(ModificationType.MaxSpeed, data.GetModificationPlace(ModificationType.MaxSpeed));
            DrawLine(ModificationType.Acceleration, data.GetModificationPlace(ModificationType.Acceleration));
            DrawLine(ModificationType.Turn, data.GetModificationPlace(ModificationType.Turn));
        }

        public void DrawLine(ModificationType modType, Transform obj)
        {
            switch (modType)
            {
                case ModificationType.MaxSpeed:
                    _speed.DrawLine(obj);
                    break;
                case ModificationType.Turn:
                    _turnSpeed.DrawLine(obj);
                    break;
                case ModificationType.Acceleration:
                    _acceleration.DrawLine(obj);
                    break;
            }
        }
    }

    public class GarageCarModel : UIModel
    {
        public CarPrefabData CarPrefabData { get; set; }

        public int SpeedLvl { get; set; } = 0;
        public int SpeedCost { get; set; } = -1;

        public int AccelerationLvl { get; set; } = 0;
        public int AccelerationCost { get; set; } = -1;

        public int TurnSpeedLvl { get; set; } = 0;
        public int TurnCost { get; set; } = -1;
    }
}
