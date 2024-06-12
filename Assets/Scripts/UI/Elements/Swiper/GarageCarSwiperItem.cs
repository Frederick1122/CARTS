using Managers.Libraries;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Tools;
using UnityEngine;

namespace Swiper
{
    public class GarageCarSwiperItem : SwiperItem
    {
        [SerializeField] 
        private TMP_Text _name;
        [SerializeField]
        private RarityPainter _rarityPointer;

        public override void InsertData(SwiperData data)
        {
            base.InsertData(data);
            _rarityPointer.SetColor(data.Rarity);
            _name.text = data.Name;
        }
    }
}
