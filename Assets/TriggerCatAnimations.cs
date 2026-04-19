using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using Random = UnityEngine.Random;

public class TriggerCatAnimations : MonoBehaviour
{
    public Animator catAnimator;
    private string catState;
    private enum CatMood { Idle, Irritated, Scared, Relaxed }
    private CatMood[] currMoods;
    private int currentSession = 0;
    private bool isPet = false;
    public GameObject sessionCompleteUI;
    public Text responseText;
    private CatMood lastMood;

    private void Start()
    {
        currMoods = generateMoods();
    }
    private CatMood[] generateMoods()
    {
        CatMood[] allMoods = { CatMood.Irritated, CatMood.Scared, CatMood.Relaxed };
        for (int i = allMoods.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (allMoods[i], allMoods[j]) = (allMoods[j], allMoods[i]);
        }
        return allMoods;
    }
    private void triggerMood(CatMood mood)
    {
        switch (mood)
        {
            case CatMood.Irritated: triggerIrritated(); break;
            case CatMood.Scared: triggerScared(); break;
            case CatMood.Relaxed: triggerRelaxed(); break;
        }
    }
    public void StartNextSession()
    {
        sessionCompleteUI.SetActive(false);
        if (currentSession >= currMoods.Length)
        {
            Debug.Log("All sessions complete");
            return;
        }

        CatMood mood = currMoods[currentSession];
        lastMood = currMoods[currentSession];
        currentSession++;

        Debug.Log($"Session {currentSession} - Mood: {mood}");
        triggerMood(mood);
    }

    public void detectTouch()
    {
        isPet = true;
        string message = lastMood switch
        {
            CatMood.Irritated => "cat was irritated",
            CatMood.Scared => "cat was scared",
            CatMood.Relaxed => "cat was relaxed"
        };
        responseText.text = message;
        sessionCompleteUI.SetActive(true);
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

}
