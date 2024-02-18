using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Swiper
{
    public abstract class SwiperItem : MonoBehaviour, IPointerClickHandler
    {
        public event Action<SwiperData> OnClick;
        public SwiperData Data { get; private set; }

        public virtual void Init() { }

        public virtual void InsertData(SwiperData data) =>
            Data = data;

        public void OnPointerClick(PointerEventData eventData) =>
            OnClick?.Invoke(Data);
    }
}
