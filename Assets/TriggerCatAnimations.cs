using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TriggerAnimations : MonoBehaviour
{
    public Animator catAnimator;
    private string catState;

    

    private void triggerAnimation(string newState, float crossfade = 0.2f, int layer = 1)
    {
        if (catState != newState)
        {
            catState = newState;
            catAnimator.CrossFade(newState, crossfade, layer);
        }
    }

    void Update()
    {
        // 0 - Reset
        if (Keyboard.current.digit0Key.wasPressedThisFrame)
        {
            triggerAnimation("idle", 0.2f, 1);
            triggerAnimation("base_back", 0.2f, 0);
            triggerAnimation("base_ears", 0.2f, 2);
        }
        // 1 - Irritated
        else if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            triggerAnimation("tail_swish",0.2f, 1);
            triggerAnimation("ears_back", 0.2f, 2);
        }

        // 2 - Scared
        else if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            triggerAnimation("tail_scared", 0.2f, 1);
            triggerAnimation("back_arch", 0.2f, 0);
        }

        // 3 - Shy
        else if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            triggerAnimation("tail_tuck", 0.2f, 1);
            triggerAnimation("shy_body", 0.2f, 0);
        }

        // 4 - Relaxed
        else if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            triggerAnimation("tail_up_relaxed", 0.2f, 1);
        }
    }

}
