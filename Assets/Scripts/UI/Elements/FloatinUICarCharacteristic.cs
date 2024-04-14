using Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Elements
{
    public class FloatingUICarCharacteristic : MonoBehaviour
    {
        public event Action<ModificationType> OnCharacteristicUpgrade = delegate { };

        [field: SerializeField] public ModificationType ModificationType { get; private set; } 

        [SerializeField] private Button _upgradeButton;
        [SerializeField] private TMP_Text _cost;

        [Header("Upgrade visual")]
        [SerializeField] private Color32 _upgradeColor;
        [SerializeField] private Image _levelImagePrefab;
        [SerializeField] private Transform _levelImageParent;

        private List<Image> _levelsImage = new();

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

        public void UpdateInfo(int level, int maxLevel, int cost)
        {
            UpdateLevelImage(maxLevel);

            for (int i = 0; i < level; i++)
                _levelsImage[i].color = _upgradeColor;

            var costString = cost.ToString();
            _upgradeButton.enabled = true;
            if (cost < 0)
            {
                costString = "Max level";
                _upgradeButton.enabled = false;
            }
            _cost.text = costString;
        }

        private void UpdateLevelImage(int maxLevel)
        {
            foreach (var image in _levelsImage)
            {
                Destroy(image.gameObject);
            }
            _levelsImage.Clear();

            for (int i = 0; i < maxLevel; i++)
            {
                var image = Instantiate(_levelImagePrefab, _levelImageParent);
                _levelsImage.Add(image);
            }
            _levelsImage.Reverse();
        }
    }
}
