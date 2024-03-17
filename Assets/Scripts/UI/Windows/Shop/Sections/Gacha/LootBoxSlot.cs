using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.Shop.Sections.Gacha
{
    public class LootBoxSlot : MonoBehaviour
    {
        public event Action<int> OnOpenLootBox;

        [SerializeField] private Image _lootBoxImg;
        [SerializeField] private Button _openLootBoxButton;

        private int _lootBoxNumber = -1;

        public void Init(int lootBoxNumber)
        {
            _lootBoxNumber = lootBoxNumber;

            _openLootBoxButton.onClick.AddListener(RequestForOpenLootBox);
        }

        private void OnDestroy() => 
            _openLootBoxButton.onClick.RemoveListener(RequestForOpenLootBox);

        public void SetUpImage(Sprite image)
        {
            if (image != null)
            {
                _openLootBoxButton.enabled = true;
                MakeButtonChosenVisual(_openLootBoxButton);
            }
            else
            {
                _openLootBoxButton.enabled = false;
                MakeButtonUnChosenVisual(_openLootBoxButton);
            }
            _lootBoxImg.sprite = image;
        }

        private void MakeButtonUnChosenVisual(Button button)
        {
            var colorBlock = button.colors;
            colorBlock.normalColor = Color.grey;
            colorBlock.selectedColor = Color.grey;
            colorBlock.pressedColor = Color.grey;
            button.colors = colorBlock;
        }

        private void MakeButtonChosenVisual(Button button)
        {
            var colorBlock = button.colors;
            colorBlock.normalColor = Color.white;
            colorBlock.selectedColor = Color.grey;
            colorBlock.pressedColor = Color.grey;
            button.colors = colorBlock;
        }

        private void RequestForOpenLootBox() => OnOpenLootBox?.Invoke(_lootBoxNumber);
    }
}
