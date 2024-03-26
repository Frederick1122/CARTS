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

        public bool IsOccupied = false;

        [SerializeField] private Image _lootBoxImg;
        [SerializeField] private Button _openLootBoxButton;

        private int _lootBoxNumber = -1;
        private Image _buttonImage;

        public void Init(int lootBoxNumber)
        {
            _lootBoxNumber = lootBoxNumber;

            _openLootBoxButton.onClick.AddListener(RequestForOpenLootBox);
            _buttonImage = _openLootBoxButton.GetComponent<Image>();
        }

        private void OnDestroy() => 
            _openLootBoxButton.onClick.RemoveListener(RequestForOpenLootBox);

        public void SetUpImage(Sprite image)
        {
            IsOccupied = image != null;
            ChangeButtonCondition(image != null);
            _lootBoxImg.sprite = image;
        }

        public bool TryChangeButtonCondition(bool condition)
        {
            condition = IsOccupied != condition ? IsOccupied : condition;
            ChangeButtonCondition(condition);
            return condition;
        }

        public void ChangeButtonCondition(bool condition)
        {
            if (condition)
            {
                MakeButtonChosenVisual(_openLootBoxButton);
                _openLootBoxButton.enabled = true;
            }
            else
            {
                MakeButtonUnChosenVisual(_openLootBoxButton);
                _openLootBoxButton.enabled = false;
            }
        }

        private void MakeButtonUnChosenVisual(Button button)
        {
            var colorBlock = button.colors;
            colorBlock.normalColor = Color.grey;
            colorBlock.selectedColor = Color.grey;
            colorBlock.pressedColor = Color.grey;
            button.colors = colorBlock;
            _buttonImage.color = Color.grey;
        }

        private void MakeButtonChosenVisual(Button button)
        {
            var colorBlock = button.colors;
            colorBlock.normalColor = Color.white;
            colorBlock.selectedColor = Color.grey;
            colorBlock.pressedColor = Color.grey;
            button.colors = colorBlock;
            _buttonImage.color = Color.white;
        }

        private void RequestForOpenLootBox() => OnOpenLootBox?.Invoke(_lootBoxNumber);
    }
}
