using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class TriggerCatAnimations : MonoBehaviour
{
    public Animator catAnimator;
    private string catState;
   
    private void triggerAnimation(string newState, float crossfade = 0.4f, int layer = 1)
    {
        if (catState != newState)
        {
            catState = newState;
            catAnimator.CrossFade(newState, crossfade, layer);
        }
    }
    void Update()
    {
        if (Keyboard.current.digit0Key.wasPressedThisFrame)
        {
            resetAnimation();
        }
        else if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            triggerIrritated();
        }
        else if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            triggerScared();
        }
        else if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            triggerRelaxed();
        }
    }

    private void triggerIrritated()
    {
        triggerAnimation("tail_swish", 0.4f, 1);
        triggerAnimation("ears_back", 0.4f, 2);
        triggerAnimation("base_back", 0.4f, 0);
    }

    private void resetAnimation()
    {
        triggerAnimation("idle", 0.4f, 1);
        triggerAnimation("base_back", 0.4f, 0);
        triggerAnimation("base_ears", 0.4f, 2);
    }
    private void triggerScared()
    {
        triggerAnimation("tail_scared", 0.4f, 1);
        triggerAnimation("back_arch", 0.4f, 0);
        triggerAnimation("base_ears", 0.4f, 2);
}

    private void triggerRelaxed()
    {
        triggerAnimation("tail_up_relaxed", 0.4f, 1);
        triggerAnimation("base_back", 0.4f, 0);
        triggerAnimation("base_ears", 0.4f, 2);
    
}
}
