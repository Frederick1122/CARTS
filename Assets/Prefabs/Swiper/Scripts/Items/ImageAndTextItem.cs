using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Swiper
{
    public class ImageAndTextItem : SwiperItem
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _name;

        public override void InsertData(SwiperData data)
        {
            base.InsertData(data);
            _image.sprite = data.Icon;
            _name.text = data.Name;
        }
    }
}
