using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Swiper
{
    public class SwiperController : MonoBehaviour
    {
        [SerializeField] private bool _isFree = false;

        public event Action<SwiperData> OnTabSelected;
        public event Action<SwiperData> OnTabClick;

        public SwiperData SelectedData
        {
            get { return GetDataByIndex(_swipeSnapMenu.SelectedTabIndex); }
        }

        public int SelectedTab => _swipeSnapMenu.SelectedTabIndex;
        public int ElementsCount => _items.Count;

        [SerializeField] private SwipeMenuType _swipeMenuType;

        [Header("SwipeSnapMenu Attribute")]
        [SerializeField] private SwipeSnapMenu _swipeSnapMenu;
        [SerializeField] private RectTransform _contentContainer;
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private SwiperItem _itemPrefab;

        [Header("Characteristic")]
        [SerializeField] private float _snapSpeed = 15f;
        [SerializeField] private float _spacing = 0f;

        [Header("UI")]
        [SerializeField] private Button _nextItemButton;
        [SerializeField] private Button _previousItemButton;

        private readonly List<SwiperItem> _items = new();
        private readonly Dictionary<int, SwiperItem> _swiperItems = new();

        public void Init()
        {
            switch (_swipeMenuType)
            {
                case SwipeMenuType.Horizontal:
                    MakeHorizontal();
                    break;

                case SwipeMenuType.Vertical:
                    MakeVertical();
                    break;
            }

            _swipeSnapMenu.OnTabSelected += TabSelected;

            SubUI();
        }

        private void OnDestroy() =>
            UnSubUI();

        private void TabSelected(int value) =>
            OnTabSelected?.Invoke(GetDataByIndex(value));

        public void SelectTab(int num) =>
            _swipeSnapMenu.SelectTab(num);

        #region Direction
        public void MakeHorizontal()
        {
            if (_contentContainer.TryGetComponent(out VerticalLayoutGroup wrongGroup))
                Destroy(wrongGroup);

            if (!_contentContainer.TryGetComponent(out HorizontalLayoutGroup layoutGroup))
                layoutGroup = _contentContainer.AddComponent<HorizontalLayoutGroup>();

            layoutGroup.childForceExpandWidth = false;
            layoutGroup.childControlHeight = false;
            layoutGroup.childControlWidth = false;
            layoutGroup.spacing = _spacing;

            _scrollRect.vertical = false;
            _scrollRect.horizontal = true;
            _swipeSnapMenu.Init(_contentContainer, _scrollRect.horizontalScrollbar, _snapSpeed);
        }

        public void MakeVertical()
        {
            if (_contentContainer.TryGetComponent(out HorizontalLayoutGroup wrongGroup))
                Destroy(wrongGroup);

            if (!_contentContainer.TryGetComponent(out VerticalLayoutGroup layoutGroup))
                layoutGroup = _contentContainer.AddComponent<VerticalLayoutGroup>();

            layoutGroup.childControlHeight = false;
            layoutGroup.childControlHeight = false;
            layoutGroup.childControlWidth = false;
            layoutGroup.spacing = _spacing;

            _scrollRect.vertical = true;
            _scrollRect.horizontal = false;
            _swipeSnapMenu.Init(_contentContainer, _scrollRect.verticalScrollbar, _snapSpeed, 1);
        }
        #endregion

        #region UI
        public void SubUI()
        {
            _nextItemButton?.onClick.AddListener(SlideNext);

            _previousItemButton?.onClick.AddListener(SlidePrevious);

            foreach (var item in _items)
                item.OnClick += OnTabClick;
        }

        public void UnSubUI()
        {
            _nextItemButton?.onClick.RemoveListener(SlideNext);
            _previousItemButton?.onClick.RemoveListener(SlidePrevious);

            foreach (var item in _items)
                item.OnClick -= OnTabClick;
        }

        private void SlideNext() =>
            _swipeSnapMenu.SelectTab(SelectedTab + 1);

        private void SlidePrevious() =>
            _swipeSnapMenu.SelectTab(SelectedTab - 1);
        #endregion

        private SwiperData GetDataByIndex(int value)
        {
            return _swiperItems[value].Data;
        }

        public void AddItems(SwiperData data)
        {
            var curTab = SelectedTab;

            var item = Instantiate(_itemPrefab, _contentContainer);
            item.Init();
            item.InsertData(data);
            item.OnClick += OnTabClick;

            _swiperItems.Add(_items.Count, item);
            _items.Add(item);

            item.AddComponent<AutoScaleItem>().Init(this, _scrollRect.viewport, _isFree, _swipeMenuType);
            TryRecalculatePositions();

            SelectTab(curTab);
        }

        public void Clear()
        {
            SelectTab(0);
            for (int i = 0; i < _items.Count; i++)
            {
                var item = _items[i];
                item.OnClick -= OnTabClick;
                DestroyImmediate(item.gameObject);
            }

            _items.Clear();
            _swiperItems.Clear();
            TryRecalculatePositions();
        }

        private void TryRecalculatePositions()
        {
            //if (_isFree)
            //    return;

            _swipeSnapMenu.RecalculatePositions();
        }
    }

    public enum SwipeMenuType
    {
        Horizontal = 0,
        Vertical = 1
    }
}
