using Managers.Libraries;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Windows.Shop
{
    public class ShopItemView : UIView, IPointerClickHandler
    {
        public event Action OnSelectCarAction;

        [SerializeField] private Image _contentImage;
        [SerializeField] private Image _lockImage;
        [SerializeField] private GameObject _priceParent;

        [SerializeField] private TMP_Text _priceText;

        public override void Init(UIModel uiModel)
        {
            var castModel = (ShopItemModel)uiModel;
            _priceText.text = castModel.Price.ToString();
            _contentImage.sprite = castModel.Icon;

            if (castModel.Purchased)
                UnLock();
            else
                Lock();
        }

        public void OnPointerClick(PointerEventData eventData) => OnSelectCarAction?.Invoke();

        public override void UpdateView(UIModel uiModel)
        {
            var castModel = (ShopItemModel)uiModel;
            if (castModel.Purchased)
                UnLock();
            else
                Lock();
        }

        private void Lock()
        {
            _priceParent.SetActive(true);
            _lockImage.gameObject.SetActive(true);
        }

        private void UnLock()
        {
            _priceParent.SetActive(false);
            _lockImage.gameObject.SetActive(false);
        }
    }

    public class ShopItemModel : UIModel
    {
        public int Price = 0;
        public Sprite Icon;
        public bool Purchased;

        public bool isSelectedCar = false;
        public string configKey = "";

        public ShopItemModel() { }

        public ShopItemModel(string configKey, bool isSelectedCar)
        {
            this.configKey = configKey;
            this.isSelectedCar = isSelectedCar;
        }

        public ShopItemModel(string configKey, bool isSelectedCar, bool purchased, Sprite icon, int price)
        {
            this.configKey = configKey;
            this.isSelectedCar = isSelectedCar;

            Price = price;
            Icon = icon;
            Purchased = purchased;
        }


    }
}