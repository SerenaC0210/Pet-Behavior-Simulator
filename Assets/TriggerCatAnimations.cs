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
    private enum CatMood { Irritated, Scared, Relaxed }
    private CatMood[] currMoods;
    private string[] layerStates = new string[3];
    private int currentSession = 0;
    private bool isPet = false;
    public GameObject sessionUI;
    public GameObject animalSelectUI;
    public Text responseText;
    public Text sessionText;
    private CatMood lastMood;
    public Button contButton;
    private bool sessionStarted = false;
    private bool sessionEnded = false;

    public Text timerText;
    public float sessionDuration = 10f;
    private float timeRemaining;
    private bool timerRunning = false;

    private void Start()
    {
        triggerIdle();
        currMoods = generateMoods();
    }

    private void Update()
    {
        if (!sessionStarted || sessionEnded) return;

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;

        }
        else
        {
            timeRemaining = 0;
            timerRunning = false;
            sessionEnded = true;
            if (!isPet) displayMessage();
        }
        updateTimerText();
    }

    private void updateTimerText()
    {
        if (!sessionStarted) return;

        int seconds = Mathf.CeilToInt(timeRemaining);
        timerText.text = "Session time: " + seconds.ToString();
    }

    private void startTimer()
    {
        timeRemaining = sessionDuration;
        timerRunning = true;
    }

    private void stopTimer()
    {
        timerRunning = false;
        timeRemaining = 0;
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
            responseText.text = "Do/don't interact with the cat based on its body language.";
            contButton.interactable = false;
            isPet = false;
            StartNextSession();
        }
        
    }

    public void StartNextSession()
    {
        sessionStarted = true;
        sessionEnded = false;
        isPet = false;
        if (currentSession >= currMoods.Length)
        {
            responseText.text = "All sessions complete!";
            return;
        }

        lastMood = currMoods[currentSession];
        CatMood mood = currMoods[currentSession];
        
        currentSession++;
        setSessionText();

        triggerMood(mood);
        startTimer();
    }


    public void setSessionText()
    {
        sessionText.text = "Session " + currentSession;
    }

    public void setIsPet()
    {
        Debug.Log("Pet is called");
        if (sessionEnded || !sessionStarted) return;

        isPet = true;
        sessionEnded = true;
        timeRemaining = 0;
        timerText.text = "Session time: 0";
        displayMessage();
    }

    public void displayMessage()
    {
        Debug.Log("inside display message. session is " + currentSession + " and lastmood was " + lastMood + ". timerRunning is " + timerRunning);
        updateTimerText();

        string message = "";
        contButton.interactable = true;

        if (lastMood == CatMood.Irritated)

        {
            message = isPet
                ? "A cat's ears flattening indicates fear/anger. A swishing tail indicates also a cat is irritated, so you shouldn't pet it."
                : "Correct! A cat's ears flattening indicates fear/anger. A swishing tail indicates also a cat is irritated, so you shouldn't pet it.";
        }
        else if (lastMood == CatMood.Relaxed)
        {
            message = "A tail that's up, but not completely rigid, indicates the cat is relaxed. You may pet it if you'd like!.";
        }
        else if (lastMood == CatMood.Scared)
        {
            message = isPet
                ? "A tail that's up, but not completely rigid, indicates the cat is relaxed. You may pet it if you'd like!."
                : "Correct! A tail that's up, but not completely rigid, indicates the cat is relaxed. You may pet it if you'd like!.";
        }
        responseText.text = message;
        sessionStarted = false;
        sessionEnded = true;
    }

    public void RestartSessions()
    {
        isPet = false;
        currentSession = 0;
        sessionStarted = false;
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
