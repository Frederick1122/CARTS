using UnityEngine;

namespace Swiper
{
    public class SwiperData
    {
        public string Key { get;}

        public Sprite Icon { get;}
        public string Name { get;}

        public SwiperData(string key, Sprite icon = null, string name = "")
        {
            Key = key;
            Icon = icon;
            Name = name;
        }
    }
}
