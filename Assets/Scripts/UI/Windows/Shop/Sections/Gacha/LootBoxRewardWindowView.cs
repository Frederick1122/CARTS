using ConfigScripts;
using DG.Tweening;
using Managers;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.Shop.Sections.Gacha
{
    [RequireComponent(typeof(RectTransform))]
    public class LootBoxRewardWindowView : UIView
    {
        public event Action OnClaim;

        [SerializeField] private Image _inputLocker;
        [SerializeField] private Image _mainBackGround;

        [Header("Card")]
        [SerializeField] private Image _icon;
        [SerializeField] private Image _background;
        [SerializeField] private Image _isDuplicateShower;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private Button _claimButton;

        [Header("Animation")]
        [SerializeField] private float _animSpeed = 0.5f;

        private RectTransform _rectTransform;

        public override void Init(UIModel uiModel)
        {
            //_rectTransform = GetComponent<RectTransform>();

            _claimButton.onClick.AddListener(RequestForClaim);
        }

        private void OnDestroy() =>
            _claimButton.onClick.RemoveListener(RequestForClaim);

        public override void Show()
        {
            base.Show();
            _inputLocker.DOFade(0.5f, _animSpeed);
            _mainBackGround.rectTransform.DOScale(Vector3.one, _animSpeed);
            //_background.DOFade(1, _animSpeed);
            //_rectTransform.DOScale(Vector3.one, _animSpeed);
        }

        public override void Hide()
        {
            _inputLocker.DOFade(0, _animSpeed);
            _mainBackGround.rectTransform.DOScale(Vector3.zero, _animSpeed);
            //_rectTransform.DOScale(Vector3.zero, _animSpeed);
            base.Hide();
        }

        public override void UpdateView(UIModel uiModel) =>
            SetUpWindow((LootboxRewardWindowModel)uiModel);

        private void SetUpWindow(LootboxRewardWindowModel model)
        {
            if (model == null)
                return;

            var config = model.CarConfig;
            _icon.sprite = config.CarIcon;
            _background.color = CarConfig.RarityColors[config.Rarity];
            _name.text = config.configName;

            var isDuplicate = PlayerManager.Instance.TryGetPurchasedCarData(config.configKey, out CarData _);
            _isDuplicateShower.enabled = isDuplicate;
        }

        private void RequestForClaim() => OnClaim?.Invoke();
    }

    public class LootboxRewardWindowModel : UIModel
    {
        public CarConfig CarConfig { get; }

        public LootboxRewardWindowModel(CarConfig carConfig) {  CarConfig = carConfig; }
    }
}
