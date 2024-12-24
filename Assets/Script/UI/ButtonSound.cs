using System;
using UnityEngine.EventSystems;
using Yd.Gameplay.Object;

namespace Script.UI
{
    public class ButtonSound : Actor, IPointerEnterHandler, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            PointerClick?.Invoke();
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            PointerEnter?.Invoke();
        }

        public event Action PointerClick;
        public event Action PointerEnter;
    }
}