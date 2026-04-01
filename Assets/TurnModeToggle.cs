using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning;

public class TurnModeToggle : MonoBehaviour
{
    public SnapTurnProvider snapTurn;
    public ContinuousTurnProvider smoothTurn;

    [Header("True = Snap | False = Smooth")]
    public bool useSnapTurn = true;

    public void ApplyMode()
    {
        if (snapTurn != null)
            snapTurn.enabled = useSnapTurn;

        if (smoothTurn != null)
            smoothTurn.enabled = !useSnapTurn;
    }

    // This is what your UI Toggle calls
    public void SetSnapTurn(bool value)
    {
        useSnapTurn = value;
        ApplyMode();
    }

    // Optional: button toggle instead of UI toggle
    public void Toggle()
    {
        useSnapTurn = !useSnapTurn;
        ApplyMode();
    }

    void Start()
    {
        ApplyMode();
    }
}