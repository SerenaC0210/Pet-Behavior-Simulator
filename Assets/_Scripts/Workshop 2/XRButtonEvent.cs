using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;

public class XRButtonEvent : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Input data that will trigger the event when pressed.")]
    XRInputButtonReader m_ButtonInput = new XRInputButtonReader("Button");

    /// <summary>
    /// Input data that will trigger the event when pressed.
    /// If the source is an Input Action, it must be button-like
    /// (performed phase when pressed).
    /// </summary>
    public XRInputButtonReader buttonInput
    {
        get => m_ButtonInput;
        set => XRInputReaderUtility.SetInputProperty(ref m_ButtonInput, value, this);
    }

    [Header("Events")]
    public UnityEvent onPressed;

    private bool wasPressed;

    private void Update()
    {
        bool isPressed = m_ButtonInput.ReadValue() > 0f;

        if (isPressed && !wasPressed)
        {
            onPressed.Invoke();
        }

        wasPressed = isPressed;
    }
}
