using ConfigScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tools
{
    public class RarityPainter : MonoBehaviour
    {
        [SerializeField]
        private List<Image> _forColoredImage = new();

        private  readonly Dictionary<Rarity, Color32> _rarityColors = new()
        {
            {Rarity.Default, Color.white},
            {Rarity.Common, Color.grey },
            {Rarity.Uncommon, new Color32(51, 153, 255, 255) },
            {Rarity.Rare, new Color32(127, 0, 255, 255) },
            {Rarity.Legendary, new Color32(255, 128, 0, 255) },
        };

        public void SetColor(Rarity rarity)
        {
            foreach (var image in _forColoredImage)
                image.color = _rarityColors[rarity];
        }
    }
}
