using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

[RequireComponent(typeof(HapticImpulsePlayer))]
public class XRDefinedHaptic : MonoBehaviour
{
    public float amplitude = 0.5f;
    public float duration = 0.1f;
    public float frequency = 0f;
    
    private HapticImpulsePlayer hapticImpulsePlayer;

    private void Start()
    {
        hapticImpulsePlayer = GetComponent<HapticImpulsePlayer>();
    }

    public void PlayHaptic()
    {
        if (hapticImpulsePlayer)
        {
            hapticImpulsePlayer.SendHapticImpulse(amplitude, duration, frequency);
        }
    }
}
