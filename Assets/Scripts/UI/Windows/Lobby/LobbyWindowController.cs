using System;
using Core.FSM;
using ProjectFsms;
using UnityEngine;

namespace UI.Windows.Lobby
{
    public class LobbyWindowController : UIController<LobbyWindowView, LobbyWindowModel>
    {
        public event Action OpenShopAction;
        public event Action OpenSettingsAction;
        public event Action OpenMapSelectionAction;
        
        public override void Init()
        {
            _view.OpenShopAction += OpenShop;
            _view.OpenSettingsAction += OpenSettings;
            _view.OpenMapSelectionAction += OpenMapSelection;
            base.Init();
        }

        private void OnDestroy()
        {
            if (_view == null)
                return;
            
            _view.OpenShopAction -= OpenShop;
            _view.OpenSettingsAction -= OpenSettings;
            _view.OpenMapSelectionAction -= OpenMapSelection;
        }

        protected override LobbyWindowModel GetViewData()
        {
            return new LobbyWindowModel();
        }

        private void OpenShop()
        {
            OpenShopAction?.Invoke();
        }
        
        private void OpenSettings()
        {
            OpenSettingsAction?.Invoke();
        }
        
        private void OpenMapSelection()
        {
            OpenMapSelectionAction?.Invoke();
        }
    }
}