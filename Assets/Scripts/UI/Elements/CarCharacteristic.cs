using Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Elements
{
    public class CarCharacteristic : MonoBehaviour
    {
        public event Action<ModificationType> OnCharacteristicUpgrade = delegate { };

        [field: SerializeField] public ModificationType ModificationType { get; private set; } 

        [SerializeField] private Button _upgradeButton;
        [SerializeField] private TMP_Text _characteristicName;
        [SerializeField] private TMP_Text _level;
        [SerializeField] private TMP_Text _cost;

        public void Init()
        {
            _upgradeButton.onClick.AddListener(RequestForUpgrade);
        }

        private void OnDestroy()
        {
            _upgradeButton.onClick.RemoveListener(RequestForUpgrade);
        }

        private void RequestForUpgrade() =>
            OnCharacteristicUpgrade?.Invoke(ModificationType);

        public void UpdateInfo(int level, int cost)
        {
            _level.text = level.ToString();
            _cost.text = cost.ToString();
        }
    }
}
