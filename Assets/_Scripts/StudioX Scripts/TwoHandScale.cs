using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
public class TwoHandScale : MonoBehaviour
{
    public float scaleMultiplier = 1f;
    public float minScale = 0.1f;
    public float maxScale = 3f;

    XRGrabInteractable grab;
    Vector3 initialScale;
    float initialHandDistance;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        grab.selectMode = InteractableSelectMode.Multiple;
        grab.trackScale = false;

        if (grab.movementType == XRBaseInteractable.MovementType.Kinematic)
            grab.movementType = XRBaseInteractable.MovementType.Instantaneous;

        initialScale = transform.localScale;
    }

    void Update()
    {
        if (grab.interactorsSelecting.Count == 2)
        {
            var handA = grab.interactorsSelecting[0].transform;
            var handB = grab.interactorsSelecting[1].transform;

            float currentDistance = Vector3.Distance(handA.position, handB.position);

            if (initialHandDistance == 0)
            {
                initialHandDistance = currentDistance;
                initialScale = transform.localScale;
            }

            float scaleFactor = currentDistance / initialHandDistance;
            float targetScale = Mathf.Clamp(
                initialScale.x * scaleFactor * scaleMultiplier,
                minScale,
                maxScale
            );

            transform.localScale = Vector3.one * targetScale;
        }
        else
        {
            initialHandDistance = 0;
        }
    }
}
