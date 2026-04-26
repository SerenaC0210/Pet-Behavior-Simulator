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
        if (timerRunning)
        {
            timeRemaining -= Time.deltaTime;

            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                timerRunning = false;
                onTimerEnd();
            }

            updateTimerText();
        }
    }

    private void updateTimerText()
    {
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
    }

    private void onTimerEnd()
    {
        detectTouch();
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
            StartNextSession();
        }
        if (timerRunning)
        {
            contButton.interactable = false;
        }
    }

    public void StartNextSession()
    {
        isPet = false;
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
        startTimer();
    }

    public void setIsPet()
    {
        isPet = true;
    }

    public void setSessionText()
    {
        sessionText.text = "Session " + currentSession;
    }

    public void detectTouch()
    {
        if (isPet)
        {
            stopTimer();
            contButton.interactable = true;
            return;
        }

        isPet = true;
        stopTimer();
        contButton.interactable = true;

        string message = "";
        if (lastMood == CatMood.Irritated)
        {
            if (timeRemaining <= 0 || timerRunning == false)
            {
                message = "Correct! A cat's ears flattening indicates fear/anger. A swishing tail indicates also a cat is irritated, so you shouldn't pet it.";
            }
            else
            {
                message = "A cat's ears flattening indicates fear/anger. A swishing tail also indicates a cat is irritated, so you shouldn't pet it.";
            }
        }
        else if (lastMood == CatMood.Relaxed) {
            message = "A tail that's up, but not completely rigid, indicates the cat is relaxed. You may pet it if you'd like!";
        
        }

        else if (lastMood == CatMood.Scared)
        {
            if (timeRemaining <= 0 || timerRunning == false)
            {
                message = "Correct! A cat's ears flattening indicates fear/anger. A tucked tail indicates anxiety. You should avoid petting anxious, scared, or uncomfortable cats.";
            }
            else
            {
                message = "A cat's ears flattening indicates fear/anger. A tucked tail indicates anxiety. You should avoid petting anxious, scared, or uncomfortable cats.";
            }
        }

        responseText.text = message;
        sessionUI.SetActive(true);
    }

    public void RestartSessions()
    {
        isPet = false;
        currentSession = 0;
        currMoods = generateMoods();
        resetAnimation();
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
