using Knot.Localization;
using Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.Shop.Sections.Gacha
{
    public class GachaWindowView : UIView
    {
        private const string NOT_ENOUGH_MONEY = "NOT_ENOUGH_MONEY";

        public event Action OnBuyLootBox;

        [SerializeField] private List<Button> _buyButtons;
        [SerializeField] private List<TMP_Text> _buyButtonTexts;

        public override void Init(UIModel uiModel)
        {
            base.Init(uiModel);

            PlayerManager.Instance.IncreaseCurrency(CurrencyType.Soft, 1000);

            foreach (var button in _buyButtons)
                button.onClick.AddListener(RequestToBuyLootBox);
        }

        private void OnDestroy()
        {
            foreach (var button in _buyButtons)
                button.onClick.RemoveListener(RequestToBuyLootBox);
        }

        public override void UpdateView(UIModel uiModel)
        {
            foreach (var text in _buyButtonTexts)
                text.text = ((GachaWindowModel)uiModel).Cost.ToString();
        }

        public void ChangeBuyButtonCondition(bool enabled)
        {
            if(enabled)
            {
                foreach (var button in _buyButtons)
                {
                    button.enabled = true;
                    button.MakeChosenVisual();
                }
            }
            else
            {
                foreach (var button in _buyButtons)
                {
                    button.enabled = false;
                    button.MakeUnChosenVisual();
                }
            }
        }

        private void RequestToBuyLootBox() => OnBuyLootBox?.Invoke();
    }

    public class GachaWindowModel : UIModel 
    {
        public int Cost { get; }

        public GachaWindowModel(int cost) { Cost = cost; }
    }
}
