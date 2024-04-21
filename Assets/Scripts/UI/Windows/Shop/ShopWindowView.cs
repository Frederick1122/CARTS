using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.Shop
{
    public class ShopWindowView : UIView 
    {
        public event Action OnOpenLobby = delegate { };
        public event Action OnOpenGacha = delegate { };
        public event Action OnOpenSaleShop = delegate { };
        public event Action OnOpenBattlePass = delegate { };

        [SerializeField] private Button _backButton;

        [Header("Animation")]
        [SerializeField] private float _scaleDuration = 0.5f;

        [Header("Section Buttons")]
        [SerializeField] private Button _gachaButton;
        [SerializeField] private Button _saleShopButton;
        //[SerializeField] private Button _battlePassButton;

        private Button _currentPressedButton;
        private readonly List<Button> _allSectionButtons = new();
        private readonly Dictionary<Button, RectTransform> _buttonsTransforms = new();

        public override void Init(UIModel uiModel)
        {
            _backButton.onClick.AddListener(OpenLobby);

            _gachaButton.onClick.AddListener(OpenGacha);
            _saleShopButton.onClick.AddListener(OpenSaleShop);
            //_battlePassButton.onClick.AddListener(OpenBattlePass);

            _allSectionButtons.Add(_gachaButton);
            _allSectionButtons.Add(_saleShopButton);
            //_allSectionButtons.Add(_battlePassButton);

            _buttonsTransforms.Add(_gachaButton, _gachaButton.GetComponent<RectTransform>());
            _buttonsTransforms.Add(_saleShopButton, _saleShopButton.GetComponent<RectTransform>());
            //_buttonsTransforms.Add(_battlePassButton, _battlePassButton.GetComponent<RectTransform>());

            OpenStartSection();
        }

        private void OnDestroy()
        {
            _backButton.onClick.RemoveListener(OpenLobby);

            _gachaButton.onClick.RemoveListener(OpenGacha);
            _saleShopButton.onClick.RemoveListener(OpenSaleShop);
            //_battlePassButton.onClick.RemoveListener(OpenBattlePass);
        }

        public override void Show()
        {
            base.Show();
            OpenStartSection();
        }

        public void OpenLobby() => OnOpenLobby?.Invoke();

        private void OpenGacha()
        {
            OnOpenGacha?.Invoke();
            _currentPressedButton = _gachaButton;
            ChooseSection();
        }

        private void OpenSaleShop()
        {
            OnOpenSaleShop?.Invoke();
            _currentPressedButton = _saleShopButton;
            ChooseSection();
        }

        //private void OpenBattlePass()
        //{
        //    OnOpenBattlePass?.Invoke();
        //    _currentPressedButton = _battlePassButton;
        //    ChooseSection();
        //}

        private void ChooseSection()
        {
            // Add some visual
            foreach (var button in _allSectionButtons)
            {
                if (button == _currentPressedButton)
                {
                    _currentPressedButton.enabled = false;
                    MakeButtonChosenVisual(_currentPressedButton);
                }
                else
                {
                    button.enabled = true;
                    MakeButtonUnChosenVisual(button);
                }
            }
        }

        public void OpenStartSection()
        {
            _currentPressedButton = _gachaButton;
            ChooseSection();
        }

        private void MakeButtonUnChosenVisual(Button button)
        {
            var colorBlock = button.colors;
            colorBlock.normalColor = Color.grey;
            button.colors = colorBlock;

            _buttonsTransforms[button].DOScale(Vector3.one, _scaleDuration);
        }

        private void MakeButtonChosenVisual(Button button)
        {
            var colorBlock = button.colors;
            colorBlock.normalColor = Color.white;
            button.colors = colorBlock;
            _buttonsTransforms[button].DOScale(new Vector3(1.1f, 1.1f, 1), _scaleDuration);
        }
    }

    public class ShopWindowModel : UIModel { }
}