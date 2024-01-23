using Managers.Libraries;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.Shop
{
    public class ShopCarView : UIView<ShopCarModel>
    {
        public event Action OnSelectCarAction;

        [SerializeField] private Button _selectButton;
        [SerializeField] private TMP_Text _carNameText;
        [SerializeField] private TMP_Text _selectedCarText;

        public override void Init(ShopCarModel uiModel)
        {
            base.Init(uiModel);
            _selectButton.onClick.AddListener(SelectCar);
        }

        public override void UpdateView(ShopCarModel uiModel)
        {
            base.UpdateView(uiModel);
            _carNameText.text = CarLibrary.Instance.GetConfig(uiModel.configKey).configName;
            _selectedCarText.text = uiModel.isSelectedCar ? "x" : "";
        }

        private void OnDestroy()
        {
            _selectButton?.onClick.RemoveListener(SelectCar);
        }

        private void SelectCar()
        {
            OnSelectCarAction?.Invoke(); ;
        }
    }

    public class ShopCarModel : UIModel
    {
        public bool isSelectedCar = false;
        public string configKey = "";

        public ShopCarModel() { }

        public ShopCarModel(string configKey, bool isSelectedCar)
        {
            this.configKey = configKey;
            this.isSelectedCar = isSelectedCar;
        }
    }
}