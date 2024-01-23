using Installers;
using Managers.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using UI.Elements;
using UnityEngine;
using Zenject;

namespace UI.Windows.MapSelection
{
    public class MapSelectionWindowController : UIController<MapSelectionWindowView, MapSelectionWindowModel>
    {
        public event Action OpenLobbyAction;
        public event Action GoToGameAction;

        [SerializeField] private TrackCustomScroll _trackCustomScroll;

        [Inject] private TrackData _trackData;
        private TrackModel _currentTrackModel;
        private List<TrackModel> _trackModels = new();

        public override void Init()
        {
            _view.OpenLobbyAction += OpenLobby;
            _view.GoToGameAction += GoToGame;
            _trackCustomScroll.OnSelectTrackAction += SelectedNewTrack;

            InitAllTracks();
            base.Init();
        }

        public override void UpdateView()
        {
            UpdateAllTracks();
        }

        protected override MapSelectionWindowModel GetViewData()
        {
            return new MapSelectionWindowModel();
        }

        private void OnDestroy()
        {
            if (_view != null)
            {
                _view.OpenLobbyAction -= OpenLobby;
                _view.GoToGameAction -= GoToGame;
            }

            if (_trackCustomScroll != null)
                _trackCustomScroll.OnSelectTrackAction -= SelectedNewTrack;
        }

        private void OpenLobby()
        {
            OpenLobbyAction?.Invoke();
        }

        private void GoToGame()
        {
            GoToGameAction?.Invoke();
        }

        private void SelectedNewTrack(TrackModel uiModel)
        {
            if (_currentTrackModel.configKey == uiModel.configKey)
                return;

            _trackData.configKey = uiModel.configKey;
            _currentTrackModel = uiModel;
            UpdateAllTracks();
        }

        private void InitAllTracks()
        {
            var trackConfigs = TrackLibrary.Instance.GetAllConfigs().Values.ToList();
            var currentTrackConfigKey = _trackData.configKey;
            foreach (var trackConfig in trackConfigs)
            {
                var isSelectedTrack = trackConfig.configKey == currentTrackConfigKey;
                _trackModels.Add(new TrackModel(trackConfig.configKey, isSelectedTrack));

                if (isSelectedTrack)
                    _currentTrackModel = _trackModels[^1];
            }

            _trackCustomScroll.AddRange(_trackModels);
        }

        private void UpdateAllTracks()
        {
            foreach (var carModel in _trackModels)
                carModel.isSelectedCar = carModel.configKey == _currentTrackModel.configKey;

            _trackCustomScroll.HideAll();
            _trackCustomScroll.AddRange(_trackModels);
        }
    }
}