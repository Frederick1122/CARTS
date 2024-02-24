using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Swiper
{
    public class SwipeSnapMenu : MonoBehaviour, IBeginDragHandler, IEndDragHandler
    {
        public event Action<int> OnTabSelected;
        public event Action<int> OnTabSnapped;

        public int SelectedTabIndex => _selectedTabIndex;

        // Serialized
        private RectTransform _contentContainer;
        private Scrollbar _scrollbar;
        private float _snapSpeed = 15f;

        //
        private bool _isSnapping;
        private bool _isDragging;
        //
        private readonly List<float> _itemPositionsNormalized = new();
        private float _targetScrollBarValueNormalized = 0f;
        private float _itemSizeNormalized;
        private int _selectedTabIndex;

        private int _topValue = 0;

        public void OnBeginDrag(PointerEventData eventData)
        {
            _isDragging = true;
            _isSnapping = false;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _targetScrollBarValueNormalized = _scrollbar.value;

            _isDragging = false;
            _isSnapping = true;

            FindSnappingTabAndStartSnapping();
        }

        public void Init(RectTransform contentContainer, Scrollbar scrollbar, float snapSpeed = 15f, int top = 0)
        {
            _contentContainer = contentContainer;
            _scrollbar = scrollbar;
            _snapSpeed = snapSpeed;
            _topValue = top;

            RecalculatePositions();
        }

        private void Update()
        {
            if (_isDragging)
                return;

            if (_isSnapping)
                SnapContent();
        }

        public void RecalculatePositions()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(_contentContainer);

            _itemPositionsNormalized.Clear();

            int itemsCount = _contentContainer.childCount;
            _itemSizeNormalized = 1f / (itemsCount - 1);

            for (int i = 0; i < itemsCount; i++)
            {
                float itemPosNormal = _itemSizeNormalized * i;
                _itemPositionsNormalized.Add(itemPosNormal);
            }

            if (_topValue == 1)
                _itemPositionsNormalized.Reverse();

            SelectTab(_selectedTabIndex + 1);
        }

        public void SelectTab(int tabIndex)
        {
            if (tabIndex < 0 || tabIndex >= _itemPositionsNormalized.Count)
                return;

            _selectedTabIndex = tabIndex;
            _targetScrollBarValueNormalized = _itemPositionsNormalized[tabIndex];
            _isSnapping = true;

            OnTabSelected?.Invoke(tabIndex);
        }

        private void FindSnappingTabAndStartSnapping()
        {
            for (int i = 0; i < _itemPositionsNormalized.Count; i++)
            {
                float itemPosNormal = _itemPositionsNormalized[i];

                if (_targetScrollBarValueNormalized < itemPosNormal + _itemSizeNormalized / 2f
                    && _targetScrollBarValueNormalized > itemPosNormal - _itemSizeNormalized / 2f)
                {
                    SelectTab(i);
                    break;
                }
            }
        }

        private void SnapContent()
        {
            if (_itemPositionsNormalized.Count < 2)
            {
                _isSnapping = false;
                return;
            }

            float targetPosition = _itemPositionsNormalized[_selectedTabIndex];
            if (Time.deltaTime == 0)
                _scrollbar.value = targetPosition;
            else
                _scrollbar.value = Mathf.Lerp(_scrollbar.value, targetPosition, Time.deltaTime * _snapSpeed);

            if (Mathf.Abs(_scrollbar.value - targetPosition) <= 0.0001f)
            {
                _isSnapping = false;
                OnTabSnapped?.Invoke(_selectedTabIndex);
            }
        }

        public GameObject GetTab(int tabIndex)
        {
            if (tabIndex < 0 || tabIndex >= _itemPositionsNormalized.Count)
                return null;

            return _contentContainer.GetChild(tabIndex).gameObject;
        }

        public GameObject GetSelectedTab()
        {
            return GetTab(_selectedTabIndex);
        }
    }
}
