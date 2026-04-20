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
    private enum CatMood {Irritated, Scared, Relaxed}
    private CatMood[] currMoods;
    private string[] layerStates = new string[3];
    private int currentSession = 0;
    private bool isPet = false;
    public GameObject sessionUI;
    public GameObject animalSelectUI;
    public Text responseText;
    public Text sessionText;
    private CatMood lastMood;

    private void Start()
    {
        triggerIdle();
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

    public void OnContinuePressed()
    {
        if (currentSession >= currMoods.Length)
        {
            RestartSessions();
            animalSelectUI.SetActive(true);
            sessionUI.SetActive(false);
            if (catAnimator) catAnimator.gameObject.SetActive(false);
        }
        else
        {
            StartNextSession();
        }
    }

    public void StartNextSession()
    {
        if (currentSession >= currMoods.Length)
        {
            responseText.text = "All sessions complete!";
            return;
        }

        CatMood mood = currMoods[currentSession];
        lastMood = currMoods[currentSession];
        currentSession++;
        setSessionText();

        Debug.Log($"Session {currentSession} - Mood: {mood}");
        triggerMood(mood);
    }

    public void setSessionText()
    {
        sessionText.text = "Session " + currentSession;
    }

    public void detectTouch()
    {
        if (currentSession >= currMoods.Length)
        {
            return;
        }
        isPet = true;
        string message = lastMood switch
        {
            CatMood.Irritated => "cat was irritated",
            CatMood.Scared => "cat was scared",
            CatMood.Relaxed => "cat was relaxed"
        };
        responseText.text = message;
        sessionUI.SetActive(true);
    }

    public void RestartSessions()
    {
        currentSession = 0;
        currMoods = generateMoods();
        triggerIdle();
    }

    private void triggerAnimation(string newState, float crossfade = 0.4f, int layer = 0)
    {
        if (layerStates[layer] != newState)
        {
            layerStates[layer] = newState;
            catAnimator.CrossFade(newState, crossfade, layer);
        }
    }

    public void triggerIrritated()
    {
        triggerAnimation("tail_swish", 0.4f, 1);
        triggerAnimation("ears_back", 0.4f, 2);
        triggerAnimation("base_back", 0.4f, 0);
    }

    public void triggerIdle()
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
        triggerAnimation("tail_up_relaxed", 0.4f, 1);
        triggerAnimation("base_back", 0.4f, 0);
        triggerAnimation("base_ears", 0.4f, 2);
    }

    public void resetAnimation()
    {
        triggerAnimation("base_tail", 0.4f, 1);
        triggerAnimation("base_back", 0.4f, 0);
        triggerAnimation("base_ears", 0.4f, 2);
    }

}
