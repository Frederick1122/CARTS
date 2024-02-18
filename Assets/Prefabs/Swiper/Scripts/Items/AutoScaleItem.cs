using UnityEngine;
using static UnityEditor.Progress;

namespace Swiper
{
    public class AutoScaleItem : MonoBehaviour
    {
        private const float TimeBetweenScreenChangeCalculations = 0.5f;
        private float _lastScreenChangeCalculationTime = 0;

        private RectTransform _item;
        private Vector2 _lastScreenWidth;

        private RectTransform _parent;
        private SwiperController _swiper;

        private bool _isFree;

        public void Init(SwiperController swiper, RectTransform parent, bool isFree = false)
        {
            _lastScreenChangeCalculationTime = Time.time;
            _item = GetComponent<RectTransform>();
            _lastScreenWidth = new Vector2(Screen.width, Screen.height);

            _parent = parent;
            _swiper = swiper;
            _isFree = isFree;

            if (!_isFree)
                _item.sizeDelta = new Vector2(_parent.rect.width, _parent.rect.height);
            else
                _item.sizeDelta = new Vector2(_item.rect.width, _parent.rect.height);
        }

        private void Update()
        {
            if (_lastScreenWidth.x == Screen.width && _lastScreenWidth.y == Screen.height)
                return;

            if (Time.time - _lastScreenChangeCalculationTime < TimeBetweenScreenChangeCalculations)
                return;

            _lastScreenChangeCalculationTime = Time.time;

            if (!_isFree)
                _item.sizeDelta = new Vector2(_parent.rect.width, _parent.rect.height);
            else
                _item.sizeDelta = new Vector2(_item.rect.width, _parent.rect.height);

            _swiper.SelectTab(_swiper.SelectedTab);

            _lastScreenWidth = new Vector2(Screen.width, Screen.height);

            Debug.Log($"Window dimensions changed to {Screen.width}x{Screen.height}");
        }
    }
}
