using UnityEngine;
using UnityEngine.InputSystem;

public class TriggerAnimations : MonoBehaviour
{
    public Animator catAnimator;

    void Update()
    {
        // 1 - Irritated
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            ResetStates();
            catAnimator.SetBool("Irritated", true);
        }

        // 2 - Scared
        else if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            ResetStates();
            catAnimator.SetBool("Scared", true);
        }

        // 3 - Shy
        else if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            ResetStates();
            catAnimator.SetBool("Shy", true);
        }

        // 4 - Relaxed
        else if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            ResetStates();
            catAnimator.SetBool("Relaxed", true);
        }
    }
    void ResetStates()
    {
        catAnimator.SetBool("Relaxed", false);
        catAnimator.SetBool("Irritated", false);
        catAnimator.SetBool("Scared", false);
        catAnimator.SetBool("Shy", false);
    }
}