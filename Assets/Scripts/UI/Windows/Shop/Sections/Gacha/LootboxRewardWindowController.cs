using ConfigScripts;
using System;

namespace UI.Windows.Shop.Sections.Gacha
{
    public class LootboxRewardWindowController : UIController
    {
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
