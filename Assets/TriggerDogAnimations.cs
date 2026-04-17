using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class TriggerDogAnimations : MonoBehaviour
{
    public Animator dogAnimator;
    private string dogState;

    private void triggerAnimation(string newState, float crossfade = 0.4f, int layer = 1)
    {
        if (dogState != newState)
        {
            dogState = newState;
            dogAnimator.CrossFade(newState, crossfade, layer);
        }
    }
    void Update()
    {
        if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            resetAnimation();
        }
        else if (Keyboard.current.digit5Key.wasPressedThisFrame)
        {
            triggerAlert();
        }
        else if (Keyboard.current.digit6Key.wasPressedThisFrame)
        {
            triggerScared();
        }
        else if (Keyboard.current.digit7Key.wasPressedThisFrame)
        {
            triggerRelaxed();
        }
    }

    private void triggerAlert()
    {
        triggerAnimation("tail up", 0.4f, 1); 
        triggerAnimation("base body", 0.4f, 0);
        triggerAnimation("base ears", 0.4f, 2);
}

    private void resetAnimation()
    {
        triggerAnimation("idle", 0.4f, 0);
        triggerAnimation("tail base", 0.4f, 1);
        triggerAnimation("base ears", 0.4f, 2);
    }
    private void triggerScared()
    {
        triggerAnimation("tail tuck", 0.4f, 1);
        triggerAnimation("ears back", 0.4f, 2);
        triggerAnimation("body tuck", 0.4f, 0);
    }

    private void triggerRelaxed()
    {
        triggerAnimation("tail wag", 0.4f, 1); 
        triggerAnimation("base body", 0.4f, 0);
        triggerAnimation("base ears", 0.4f, 2);
    }
}
