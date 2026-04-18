using System;
using UnityEditor;
using UnityEngine;

namespace ScrollCarousel
{
    [CustomEditor(typeof(Carousel))]
    public class ScrollCarouselEditor : Editor
    {
        private Carousel carousel;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            carousel = (Carousel)target;

            GUILayout.BeginVertical("box");
            GUILayout.Label("Carousel Editor", EditorStyles.boldLabel);

            if (GUILayout.Button("Add All Children to Items"))
            {
                AddAllChildrenToItemList();
            }

            if (GUILayout.Button("Organize Items"))
            {
                OrganizeItemsInEditor();
                UpdateItemsAppearanceInEditor();
                OrganizeItemsInEditor();
            }

            GUILayout.EndVertical();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("Items"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("StartItem"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Itemspacing"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("CenteredScale"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("NonCenteredScale"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("MaxRotationAngle"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_rotationSmoothSpeed"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ColorAnimation"));

            if (serializedObject.FindProperty("ColorAnimation").boolValue)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("FocustedColor"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("NonFocustedColor"));
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void AddAllChildrenToItemList()
        {
            carousel.Items.Clear();
            foreach (Transform child in carousel.transform)
            {
                if (child is RectTransform rectTransform)
                {
                    carousel.Items.Add(rectTransform);
                }
            }
            EditorUtility.SetDirty(carousel);
        }

        private void OrganizeItemsInEditor()
        {
            if (carousel.Items.Count == 0) return;

            Vector2 centerPoint = carousel.GetComponent<RectTransform>().rect.center;

            for (int i = 0; i < carousel.Items.Count; i++)
            {
                float offset = GetTotalOffset(i) - GetTotalOffset(carousel.StartItem);
                Vector2 targetPosition = new Vector2(centerPoint.x + offset, centerPoint.y);
                carousel.Items[i].anchoredPosition = targetPosition;
            }

            EditorUtility.SetDirty(carousel);
        }

        private void UpdateItemsAppearanceInEditor()
        {
            if (carousel.Items.Count == 0) return;

            Vector2 centerPoint = carousel.GetComponent<RectTransform>().rect.center;
            float maxDistance = Mathf.Max(GetItemspacing(0), 1f);

            for (int i = 0; i < carousel.Items.Count; i++)
            {
                if (!carousel.Items[i]) continue;

                float visualDistance = Mathf.Abs(i - carousel.StartItem);
                carousel.Items[i].SetSiblingIndex(carousel.Items.Count - (int)(visualDistance * 2));

                float distance = Mathf.Abs(carousel.Items[i].anchoredPosition.x - centerPoint.x);
                float normalizedDistance = Mathf.Clamp01(distance / maxDistance);

                float targetScale = i == carousel.StartItem ? carousel.CenteredScale : carousel.NonCenteredScale;
                Vector3 newScale = new Vector3(targetScale, targetScale, 1f);
                if (!float.IsNaN(newScale.x) && !float.IsNaN(newScale.y))
                {
                    carousel.Items[i].localScale = newScale;
                }

                float rotationSign = (carousel.Items[i].anchoredPosition.x > centerPoint.x) ? 1f : -1f;
                float targetRotationY = carousel.MaxRotationAngle * normalizedDistance * rotationSign;
                if (!float.IsNaN(targetRotationY))
                {
                    carousel.Items[i].localRotation = Quaternion.Euler(0, targetRotationY, 0);
                }
            }

            EditorUtility.SetDirty(carousel);
        }

        private float GetItemspacing(int index)
        {
            if (index < 0 || index >= carousel.Items.Count - 1)
                return carousel.Itemspacing;

            float currentItemscale = (index == carousel.StartItem) ? carousel.CenteredScale : carousel.NonCenteredScale;
            float nextItemscale = (index + 1 == carousel.StartItem) ? carousel.CenteredScale : carousel.NonCenteredScale;

            float currentWidth = carousel.Items[index].rect.width * currentItemscale;
            float nextWidth = carousel.Items[index + 1].rect.width * nextItemscale;

            return (currentWidth + nextWidth) / 2 + carousel.Itemspacing;
        }

        private float GetTotalOffset(int index)
        {
            float offset = 0f;
            int startIdx = Math.Min(index, carousel.StartItem);
            int endIdx = Math.Max(index, carousel.StartItem);

            for (int i = startIdx; i < endIdx; i++)
            {
                offset += GetItemspacing(i);
            }

            return index < carousel.StartItem ? -offset : offset;
        }
    }
}