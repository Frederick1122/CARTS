using Swiper;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.MapSelection
{
    public class MapSelectionWindowView : UIView
    {
        public event Action OpenLobbyAction;

        public event Action<string> OnModeSwipe;

        public event Action<string> OnModSelect;
        public event Action<string> OnMapSelect;

        [Header("Base")]
        [SerializeField] private Button _selectionButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private TMP_Text _nameOfChosen;

        [Header("Mod Selection")]
        [SerializeField] private SwiperController _modSwiper;

        [Header("Map Selection")]
        [SerializeField] private SwiperController _mapSwiper;

        public override void Init(UIModel model)
        {
            base.Init(model);

            _modSwiper.Init();
            _mapSwiper.Init();

            _modSwiper.OnTabSelected += UpdateName;
            _modSwiper.OnTabSelected += SwipeMode;
            _mapSwiper.OnTabSelected += UpdateName;
        }

        private void OnDestroy()
        {
            _modSwiper.OnTabSelected -= UpdateName;
            _modSwiper.OnTabSelected -= SwipeMode;
            _mapSwiper.OnTabSelected -= UpdateName;
        }

        private void GoToLobby() => OpenLobbyAction?.Invoke();

        public void AddMap(SwiperData data) =>
            _mapSwiper.AddItems(data);

        public void AddMod(SwiperData data) =>
            _modSwiper.AddItems(data);

        public void ClearMapSwiper() =>
            _mapSwiper.Clear();

        public void ClearModSwiper() =>
            _modSwiper.Clear();

        private void SwipeMode(SwiperData data) => OnModeSwipe?.Invoke(data.Key);
        private void SelectMod() => OnModSelect?.Invoke(_modSwiper.SelectedData.Key);
        private void SelectMap() => OnMapSelect?.Invoke(_mapSwiper.SelectedData.Key);

        public void ShowModSelection()
        {
            _modSwiper.gameObject.SetActive(true);
            _mapSwiper.gameObject.SetActive(false);

            _selectionButton.onClick.RemoveAllListeners();
            _selectionButton.onClick.AddListener(SelectMod);

            _backButton.onClick.RemoveAllListeners();
            _backButton.onClick.AddListener(GoToLobby);
        }

        public void ShowMapSelection()
        {
            _modSwiper.gameObject.SetActive(false);
            _mapSwiper.gameObject.SetActive(true);

            _selectionButton.onClick.RemoveAllListeners();
            _selectionButton.onClick.AddListener(SelectMap);

            _backButton.onClick.RemoveAllListeners();
            _backButton.onClick.AddListener(ShowModSelection);
        }

        private void UpdateName(SwiperData data) => 
            _nameOfChosen.text = data.Name;
       
        public void OpenMode()
        {
            _selectionButton.enabled = true;
            var colorBlock = _selectionButton.colors;
            colorBlock.normalColor = Color.white;
            colorBlock.highlightedColor = Color.white;
            _selectionButton.colors = colorBlock;
        }

        public void CloseMode()
        {
            _selectionButton.enabled = false;
            var colorBlock = _selectionButton.colors;
            colorBlock.normalColor = Color.grey;
            colorBlock.highlightedColor = Color.grey;
            _selectionButton.colors = colorBlock;
        }
        
    }

    public class MapSelectionWindowModel : UIModel { }
}