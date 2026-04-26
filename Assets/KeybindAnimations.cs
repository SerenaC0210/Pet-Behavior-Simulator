using UnityEngine;
using UnityEngine.InputSystem;

public class KeybindAnimations : MonoBehaviour
{
    public Animator catAnimator;
    private string catState;
    public Animator dogAnimator;
    private string dogState;
    void Start()
    {
        resetCatAnimation();
    }

    void Update()
    {
        if (Keyboard.current.digit0Key.wasPressedThisFrame)
        {
            triggerCatAnimation("tail_tuck", 0.4f, 1);
        }
        else if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            triggerDogAnimation("ears back", 0.4f, 2);
        }
        else if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            triggerDogAnimation("tail wag", 0.4f, 1);
        }
        else if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            triggerDogAnimation("tail base", 0.4f, 1);
        }

        if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            triggerCatAnimation("back_arch", 0.4f, 0);
        }
        else if (Keyboard.current.digit5Key.wasPressedThisFrame)
        {
            triggerDogAlert();
        }
        else if (Keyboard.current.digit6Key.wasPressedThisFrame)
        {
            triggerDogScared();
        }
        else if (Keyboard.current.digit7Key.wasPressedThisFrame)
        {
            triggerDogRelaxed();
        }
    }

    private void triggerDogAnimation(string newState, float crossfade = 0.4f, int layer = 1)
    {
        if (dogState != newState)
        {
            dogState = newState;
            dogAnimator.CrossFade(newState, crossfade, layer);
        }
    }

    private void triggerDogAlert()
    {
        triggerDogAnimation("tail up", 0.4f, 1);
        triggerDogAnimation("base body", 0.4f, 0);
        triggerDogAnimation("base ears", 0.4f, 2);
    }

    private void resetDogAnimation()
    {
        triggerDogAnimation("idle", 0.4f, 0);
        triggerDogAnimation("tail base", 0.4f, 1);
        triggerDogAnimation("base ears", 0.4f, 2);
    }
    private void triggerDogScared()
    {
        triggerDogAnimation("tail tuck", 0.4f, 1);
        triggerDogAnimation("ears back", 0.4f, 2);
        triggerDogAnimation("body tuck", 0.4f, 0);
    }

    private void triggerDogRelaxed()
    {
        triggerDogAnimation("tail wag", 0.4f, 1);
        triggerDogAnimation("base body", 0.4f, 0);
        triggerDogAnimation("base ears", 0.4f, 2);
    }

    private void triggerCatAnimation(string newState, float crossfade = 0.4f, int layer = 1)
    {
        if (catState != newState)
        {
            catState = newState;
            catAnimator.CrossFade(newState, crossfade, layer);
        }
    }

    public void triggerCatIrritated()
    {
        triggerCatAnimation("tail_swish", 0.4f, 1);
        triggerCatAnimation("ears_back", 0.4f, 2);
        triggerCatAnimation("base_back", 0.4f, 0);
    }

    public void triggerCatIdle()
    {
        triggerCatAnimation("idle", 0.4f, 1);
        triggerCatAnimation("base_back", 0.4f, 0);
        triggerCatAnimation("base_ears", 0.4f, 2);
    }
    public void triggerCatScared()
    {
        triggerCatAnimation("tail_scared", 0.4f, 1);
        triggerCatAnimation("back_arch", 0.4f, 0);
        triggerCatAnimation("base_ears", 0.4f, 2);
    }

    public void triggerCatRelaxed()
    {
        triggerCatAnimation("tail_up_relaxed", 0.4f, 1);
        triggerCatAnimation("base_back", 0.4f, 0);
        triggerCatAnimation("base_ears", 0.4f, 2);
    }

    public void resetCatAnimation()
    {
        triggerCatAnimation("base_tail", 0.4f, 1);
        triggerCatAnimation("base_back", 0.4f, 0);
        triggerCatAnimation("base_ears", 0.4f, 2);
    }
}
