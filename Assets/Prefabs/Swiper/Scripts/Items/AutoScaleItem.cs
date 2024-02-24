using UnityEngine;

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
        private SwipeMenuType _menuType;

        public void Init(SwiperController swiper, RectTransform parent, 
            bool isFree = false, SwipeMenuType menuType = SwipeMenuType.Horizontal)
        {
            _lastScreenChangeCalculationTime = Time.time;
            _item = GetComponent<RectTransform>();
            _lastScreenWidth = new Vector2(Screen.width, Screen.height);

            _parent = parent;
            _swiper = swiper;
            _isFree = isFree;
            _menuType = menuType;

            _item.sizeDelta = RecalculateSize();
        }

        private void Update()
        {
            if (_lastScreenWidth.x == Screen.width && _lastScreenWidth.y == Screen.height)
                return;

            if (Time.time - _lastScreenChangeCalculationTime < TimeBetweenScreenChangeCalculations)
                return;

            _lastScreenChangeCalculationTime = Time.time;
            _lastScreenWidth = new Vector2(Screen.width, Screen.height);

            _item.sizeDelta = RecalculateSize();

            _swiper.SelectTab(_swiper.SelectedTab);
        }

        private Vector2 RecalculateSize()
        {
            var size = Vector2.zero;

            if (!_isFree)
                size = new Vector2(_parent.rect.width, _parent.rect.height);
            else
            {
                switch (_menuType)
                {
                    case SwipeMenuType.Horizontal:
                        size = new Vector2(_item.rect.width, _parent.rect.height);
                        break;
                    case SwipeMenuType.Vertical:
                        size = new Vector2(_parent.rect.width, _item.rect.height);
                        break;
                }
            }

            return size;
        }
    }
}
