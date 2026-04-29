using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using Random = UnityEngine.Random;

public class TriggerDogAnimations : MonoBehaviour
{
    public Animator dogAnimator;
    private string dogState;
    private enum DogMood { Alert, Scared, Relaxed }
    private DogMood[] currMoods;
    private string[] layerStates = new string[3];
    private int currentSession = 0;
    private bool isPet = false;
    public GameObject sessionUI;
    public GameObject animalSelectUI;
    public Text responseText;
    public Text sessionText;
    private DogMood lastMood;
    public Button contButton;
    private bool sessionStarted = false;
    private bool sessionEnded = false;


    public Text timerText;
    public float sessionDuration = 8f;
    private float timeRemaining;
    private bool timerRunning = false;

    private void Start()
    {
        resetAnimation();
        currMoods = generateMoods();
    }

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
            displayMessage();
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
    }

    private DogMood[] generateMoods()
    {
        DogMood[] allMoods = { DogMood.Alert, DogMood.Scared, DogMood.Relaxed };
        for (int i = allMoods.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (allMoods[i], allMoods[j]) = (allMoods[j], allMoods[i]);
        }
        return allMoods;
    }
    private void triggerMood(DogMood mood)
    {
        switch (mood)
        {
            case DogMood.Alert: triggerAlert(); break;
            case DogMood.Scared: triggerScared(); break;
            case DogMood.Relaxed: triggerRelaxed(); break;
        }
    }

    public void OnContinuePressed()
    {
        if (currentSession >= currMoods.Length)
        {
            RestartSessions();
            animalSelectUI.SetActive(true);
            sessionUI.SetActive(false);
            if (dogAnimator) dogAnimator.gameObject.SetActive(false);
        }
        else
        {
            responseText.text = "Do/don't interact with the dog based on its body language.";
            isPet = false;
            StartNextSession();
            contButton.interactable = false;
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
            RestartSessions();
            return;
        }

        lastMood = currMoods[currentSession];
        DogMood mood = currMoods[currentSession];

        currentSession++;
        setSessionText();

        triggerMood(mood);
        startTimer();
    }
    public void setIsPet()
    {
        isPet = true;
        Debug.Log($"Pet is called and isPet is {isPet}");
        if (sessionEnded || !sessionStarted) return;

        
        sessionEnded = true;
        timeRemaining = 0;
        timerText.text = "Session time: 0";
        displayMessage();
    }

    public void setSessionText()
    {
        sessionText.text = "Session " + currentSession;
    }


    public void displayMessage()
    {
        Debug.Log("inside display message. session is " + currentSession + " and lastmood was " + lastMood + ". timerRunning is " + timerRunning);
        updateTimerText();

        string message = "";
        contButton.interactable = true;

        if (lastMood == DogMood.Alert)

        {
            message = "A dog's tail going up and ears pointing straight up indicates the dog is alert. This is a neutral state, and depending on what happens, the dog can become happy, anxious, etc.";
        }
        else if (lastMood == DogMood.Relaxed)
        {
            message = "The dog is relaxed, indicated by its softer body language. A tail wag can indicate many emotions, but in combination with other body language, the tail indicates relaxation.";
        }
        else if (lastMood == DogMood.Scared)
        {
            message = isPet
                ? "A dog's ears flattening, its tail tucking, and its crouched stance indicates anxiety or fear. Avoid petting anxious/scared dogs."
                : "Correct! A dog's ears flattening, its tail tucking, and its crouched stance indicates anxiety or fear. Avoid petting anxious/scared dogs.";
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
