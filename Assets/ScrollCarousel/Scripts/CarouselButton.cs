using System;
using UnityEngine.UI;
using UnityEngine;

namespace ScrollCarousel
{
    public class CarouselButton : Button
    {
        [SerializeField] private Action buttonAction;
        private Carousel carousel;
        private bool isFocused = false;

        protected override void Start()
        {
            base.Start();
            carousel = GetComponentInParent<Carousel>();

            if (carousel == null)
            {
                Debug.LogWarning("CarouselButton: No ScrollCarousel found in parent.");
            }
        }

        public override void OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)
        {
            if (carousel != null)
            {
                carousel.FocusItem(this.GetComponent<RectTransform>());
            }

            base.OnPointerClick(eventData);
        }

        public void SetFocus(bool focus)
        {
            this.isFocused = focus;
        }
    }
}
