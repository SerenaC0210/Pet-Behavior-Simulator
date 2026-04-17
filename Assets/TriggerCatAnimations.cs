using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using Random = UnityEngine.Random;

public class TriggerCatAnimations : MonoBehaviour
{
    public Animator catAnimator;
    private string catState;
    private enum CatMood { Idle, Irritated, Scared, Relaxed }
    private CatMood currentMood;

    private void Start()
    {
        initializeState();
    }
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
            triggerAnimation("tail_up_relaxed", 0.4f, 1);
            triggerRelaxed();
        }
    }

    public void triggerIrritated()
    {
        triggerAnimation("tail_swish", 0.4f, 1);
        triggerAnimation("ears_back", 0.4f, 2);
        triggerAnimation("base_back", 0.4f, 0);
    }

    public void resetAnimation()
    {
        triggerAnimation("idle", 0.4f, 1);
        triggerAnimation("base_back", 0.4f, 0);
        triggerAnimation("base_ears", 0.4f, 2);
    }
    public void triggerScared()
    {
        triggerAnimation("tail_scared", 0.4f, 1);
        triggerAnimation("back_arch", 0.4f, 0);
        triggerAnimation("base_ears", 0.4f, 2);
}

    public void triggerRelaxed()
    {
        triggerAnimation("tail_relaxed_idle", 0.4f, 1);
        triggerAnimation("base_back", 0.4f, 0);
        triggerAnimation("base_ears", 0.4f, 2);
    }

    private void initializeState()
    {
        CatMood[] moods = { CatMood.Idle, CatMood.Irritated, CatMood.Scared, CatMood.Relaxed };
        currentMood = moods[Random.Range(0, moods.Length)];

        switch (currentMood)
        {
            case CatMood.Idle: resetAnimation(); break;
            case CatMood.Irritated: triggerIrritated(); break;
            case CatMood.Scared: triggerScared(); break;
            case CatMood.Relaxed: triggerRelaxed(); break;
        }
    }

    public void checkTouch()
    {

    }
}
