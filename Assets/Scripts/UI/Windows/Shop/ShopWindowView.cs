using System;
using TMPro;
using UI.Elements;
using UI.Windows.Garage;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.Shop
{
    public class ShopWindowView : GarageWindowView 
    {
        [SerializeField] private TMP_Text _name;

        private readonly ShopWindowModel _model = new();

        public void UpdateCarName(string name)
        {
            _model.CarName = name;
            UpdateView(_model);
        }

        public override void UpdateView(UIModel uiModel) => _name.text = _model.CarName;
    }

    public class ShopWindowModel : UIModel 
    {
        public string CarName = "";
    }
}