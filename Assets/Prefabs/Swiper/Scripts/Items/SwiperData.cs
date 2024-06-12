using ConfigScripts;
using UnityEngine;

namespace Swiper
{
    public class SwiperData
    {
        public string Key { get;}

        public Sprite Icon { get;}
        public string Name { get;}
        public Rarity Rarity { get;}

        public SwiperData(string key, Sprite icon = null, string name = "", Rarity rarity = Rarity.Default)
        {
            Key = key;
            Icon = icon;
            Name = name;
            Rarity = rarity;
        }
    }
}
