using UnityEngine;
using UnityEngine.UI;

namespace Swiper
{
    public class OnlyImageItem : SwiperItem
    {
        [SerializeField] private Image _image;

        public override void InsertData(SwiperData data)
        {
            base.InsertData(data);
            _image.sprite = data.Icon;
        }
    }
}
