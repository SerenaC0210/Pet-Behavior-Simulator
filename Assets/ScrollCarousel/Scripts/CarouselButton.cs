using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ScrollCarousel
{
    public class CarouselButton : Button
    {
        protected override void Start()
        {
            base.Start();
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
        }

        public void SetFocus(bool focus)
        {

        }
    }
}