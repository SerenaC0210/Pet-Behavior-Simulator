using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.ComponentModel;

namespace ScrollCarousel
{
    public class Carousel : MonoBehaviour
    {
        [Header("Items")]
        [Description("List of items to be displayed in the carousel")]
        public List<RectTransform> Items = new List<RectTransform>();

        [Header("Position")]
        [Description("Index of the item that will be centered at the start")]
        public int StartItem = 0;
        [Description("Spacing between items")]
        public float Itemspacing = 50f;

        [Header("Scale")]
        [Description("Scale of the centered item")]
        public float CenteredScale = 1f;
        [Description("Scale of the non-centered items")]
        public float NonCenteredScale = 1f;

        [Header("Rotation")]
        [Description("Maximum rotation angle of the items")]
        [SerializeField] public float MaxRotationAngle = 0f;
        [SerializeField] private float _rotationSmoothSpeed = 5f;

        [Header("Colors")]
        [Description("Enable color animation")]
        public bool ColorAnimation = false;
        [Description("Color of the focused item")]
        public Color FocustedColor = Color.white;
        [Description("Color of the non-focused items")]
        public Color NonFocustedColor = Color.white;

        private RectTransform _rectTransform;
        private int _currentItemIndex = 0;
        private Dictionary<RectTransform, Coroutine> _activeColorAnimations = new Dictionary<RectTransform, Coroutine>();

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void Start()
        {
            FocusItem(StartItem);
            ForceUpdate();
        }

        private void Update()
        {
            UpdateItemsAppearance();
        }

        private float GetItemspacing(int index)
        {
            if (index < 0 || index >= Items.Count - 1)
                return Itemspacing;

            float currentItemscale = (index == _currentItemIndex) ? CenteredScale : NonCenteredScale;
            float nextItemscale = (index + 1 == _currentItemIndex) ? CenteredScale : NonCenteredScale;

            float currentWidth = Items[index].rect.width * currentItemscale;
            float nextWidth = Items[index + 1].rect.width * nextItemscale;

            return (currentWidth + nextWidth) / 2 + Itemspacing;
        }

        private float GetTotalOffset(int index)
        {
            float offset = 0f;
            int startIdx = Math.Min(index, _currentItemIndex);
            int endIdx = Math.Max(index, _currentItemIndex);

            for (int i = startIdx; i < endIdx; i++)
            {
                offset += GetItemspacing(i);
            }

            return index < _currentItemIndex ? -offset : offset;
        }

        private void PositionItems()
        {
            if (Items.Count == 0) return;

            Vector2 centerPoint = _rectTransform.rect.center;

            for (int i = 0; i < Items.Count; i++)
            {
                float offset = GetTotalOffset(i);
                Vector2 targetPosition = new Vector2(centerPoint.x + offset, centerPoint.y);
                Items[i].anchoredPosition = targetPosition;
            }
        }

        private void UpdateItemsAppearance()
        {
            if (Items.Count == 0) return;

            Vector2 centerPoint = _rectTransform.rect.center;
            float maxDistance = Mathf.Max(GetItemspacing(0), 1f);
            float minDistance = float.MaxValue;
            int closestIndex = -1;

            for (int i = 0; i < Items.Count; i++)
            {
                if (!Items[i]) continue;

                float distance = Mathf.Abs(Items[i].anchoredPosition.x - centerPoint.x);
                float angleDistance = Mathf.Abs(i - _currentItemIndex);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestIndex = i;
                }

                Items[i].SetSiblingIndex(Items.Count - (int)(angleDistance * 2));

                float normalizedDistance = Mathf.Clamp01(distance / maxDistance);

                float targetScale = Mathf.Lerp(CenteredScale, NonCenteredScale, normalizedDistance);
                Vector3 newScale = new Vector3(targetScale, targetScale, 1f);
                if (!float.IsNaN(newScale.x) && !float.IsNaN(newScale.y))
                {
                    Items[i].localScale = newScale;
                }

                float rotationSign = (Items[i].anchoredPosition.x > centerPoint.x) ? 1f : -1f;
                float targetRotationY = MaxRotationAngle * normalizedDistance * rotationSign;
                if (!float.IsNaN(targetRotationY))
                {
                    Items[i].localRotation = Quaternion.Slerp(
                        Items[i].localRotation,
                        Quaternion.Euler(0, targetRotationY, 0),
                        Time.deltaTime * _rotationSmoothSpeed
                    );
                }
            }

            if (ColorAnimation)
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    Color targetColor = (i == closestIndex) ? FocustedColor : NonFocustedColor;
                    StartColorAnimation(Items[i], targetColor);
                }
            }
        }

        public void FocusItem(RectTransform item)
        {
            FocusItem(Items.IndexOf(item));
        }

        private void FocusItem(int index)
        {
            if (index < 0 || index >= Items.Count) return;

            _currentItemIndex = index;

            for (int i = 0; i < Items.Count; i++)
            {
                var item = Items[i];
                bool isFocused = i == _currentItemIndex;

                item.GetComponent<CarouselButton>()?.SetFocus(isFocused);
            }

            PositionItems();
        }

        public void GoToNext()
        {
            if (_currentItemIndex < Items.Count - 1)
            {
                FocusItem(_currentItemIndex + 1);
            }
        }

        public void GoToPrevious()
        {
            if (_currentItemIndex > 0)
            {
                FocusItem(_currentItemIndex - 1);
            }
        }

        public void ForceUpdate()
        {
            PositionItems();
            UpdateItemsAppearance();
        }

        private void StartColorAnimation(RectTransform item, Color targetColor)
        {
            if (_activeColorAnimations.ContainsKey(item))
            {
                StopCoroutine(_activeColorAnimations[item]);
                _activeColorAnimations.Remove(item);
            }
            _activeColorAnimations[item] = StartCoroutine(ColorAnimationCoroutine(item, targetColor));
        }

        private System.Collections.IEnumerator ColorAnimationCoroutine(RectTransform item, Color targetColor)
        {
            Image image = item.GetComponent<Image>();
            if (image == null)
            {
                _activeColorAnimations.Remove(item);
                yield break;
            }

            Color startColor = image.color;
            float elapsedTime = 0f;
            float duration = 0.2f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                image.color = Color.Lerp(startColor, targetColor, elapsedTime / duration);
                yield return null;
            }

            image.color = targetColor;
            _activeColorAnimations.Remove(item);
        }
    }
}