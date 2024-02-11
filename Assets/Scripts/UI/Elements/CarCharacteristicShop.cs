using Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI.Elements
{
    public class CarCharacteristicShop : MonoBehaviour
    {
        [field: SerializeField] public ModificationType ModificationType { get; private set; }

        [SerializeField] private TMP_Text _minLevel;
        [SerializeField] private TMP_Text _maxLevel;

        public void UpdateInfo(float min, float max)
        {
            _minLevel.text = min.ToString();
            _maxLevel.text = max.ToString();
        }
    }
}
