using System;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Elements
{
    public class CurrencyImage : MonoBehaviour
    {
        [SerializeField] private Image _regularCurrencyImage;
        [SerializeField] private Image _premiumCurrencyImage;

        public void SetImage(CurrencyType type)
        {
            HideAll();
            switch (type)
            {
                case CurrencyType.Regular:
                    _regularCurrencyImage.enabled = true;
                    break;
                case CurrencyType.Premium:
                    _premiumCurrencyImage.enabled = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private void HideAll()
        {
            _regularCurrencyImage.enabled = false;
            _premiumCurrencyImage.enabled = false;
        }
    }
}