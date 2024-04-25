using ConfigScripts;
using System;
using Managers;

namespace UI.Windows.Shop.Sections.Gacha
{
    public class LootboxRewardWindowController : UIController
    {
        private const string LOOTBOX_SOUND = "SFX/UI/LootBox";

        public event Action OnOpen;
        public event Action OnClose;

        private LootboxRewardWindowModel _model;

        public override void Init()
        {
            base.Init();
            GetView<LootBoxRewardWindowView>().OnClaim += Hide;
        }

        private void OnDestroy() =>
            GetView<LootBoxRewardWindowView>().OnClaim -= Hide;

        protected override UIModel GetViewData() { return _model; }

        public override void Show()
        {
            OnOpen?.Invoke();
            SoundManager.Instance.PlayOneShot(LOOTBOX_SOUND);
            base.Show();
        }

        public override void Hide()
        {
            OnClose?.Invoke();
            base.Hide();
        }

        public void OpenLootBox(CarConfig config)
        {
            _model = new(config);
            UpdateView();
            Show();
        }
    }
}
