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
        public event Action OnBuyLootBox;
        //public event Action<int> OnOpenLootBox;

        [SerializeField] private Button _buyButton;
        [SerializeField] private TMP_Text _buyButtonText;
        //[SerializeField] private List<LootBoxSlot> _slots = new(3);

        public override void Init(UIModel uiModel)
        {
            base.Init(uiModel);

            PlayerManager.Instance.IncreaseCurrency(CurrencyType.Soft, 1000);

            _buyButton.onClick.AddListener(RequestToBuyLootBox);
            //_buyButton.onClick.AddListener(RequestToOpenLootBox);
            _buyButtonText.text = ((GachaWindowModel)uiModel).Cost.ToString();


            //for (int slotNum = 0; slotNum < _slots.Count; slotNum++)
            //{
            //    _slots[slotNum].Init(slotNum);
            //    _slots[slotNum].OnOpenLootBox += RequestToOpenLootBox;
            //}
        }

        private void OnDestroy()
        {
            _buyButton.onClick.RemoveListener(RequestToBuyLootBox);

            //foreach (var slot in _slots) 
            //    slot.OnOpenLootBox -= RequestToOpenLootBox;
        }

        public void ChangeBuyButtonCondition(bool enabled)
        {
            if(enabled)
            {
                _buyButton.enabled = true;
                _buyButton.MakeChosenVisual();
            }
            else
            {
                _buyButton.enabled = false;
                _buyButton.MakeUnChosenVisual();
            }
        }

        //public void UpdateLootBoxImage(int slotNum, Sprite img) => 
        //    _slots[slotNum].SetUpImage(img);

        //public void TryChangeSlotButtonsCondition(bool condition)
        //{
        //    foreach (var slot in _slots)
        //        slot.TryChangeButtonCondition(condition);
        //}

        //public void ChangeSlotButtonsCondition(bool condition)
        //{
        //    foreach (var slot in _slots)
        //        slot.ChangeButtonCondition(condition);
        //}

        private void RequestToBuyLootBox() => OnBuyLootBox?.Invoke();
        //private void RequestToOpenLootBox(int slotNum) => OnOpenLootBox?.Invoke(slotNum);
    }

    public class GachaWindowModel : UIModel 
    {
        public int Cost { get; }

        public GachaWindowModel(int cost) { Cost = cost; }
    }
}
