using UnityEngine;

public class StartSessions : MonoBehaviour
{
    public Animator catAnimator;
    public Animator dogAnimator;
    public GameObject animalSelectUI;
    public GameObject catSessionUI;
    public GameObject dogSessionUI;
    public TriggerDogAnimations triggerDogAnimations;
    public TriggerCatAnimations triggerCatAnimations;
    void Start()
    {
        animalSelectUI.SetActive(true);
        catSessionUI.SetActive(false);
        dogSessionUI.SetActive(false);
        if (catAnimator) catAnimator.gameObject.SetActive(false);
        if (dogAnimator) dogAnimator.gameObject.SetActive(false);
    }

    public void ChooseCat()
    {
        catAnimator.gameObject.SetActive(true);
        dogAnimator.gameObject.SetActive(false);
        animalSelectUI.SetActive(false);
        catSessionUI.SetActive(true);
        dogSessionUI.SetActive(false);
    }

    public void ChooseDog()
    {
        dogAnimator.gameObject.SetActive(true);
        catAnimator.gameObject.SetActive(false);
        animalSelectUI.SetActive(false);
        catSessionUI.SetActive(false);
        dogSessionUI.SetActive(true);
    }

    public void restart()
    {
        animalSelectUI.SetActive(true);
        catSessionUI.SetActive(false);
        dogSessionUI.SetActive(false);
        if (catAnimator) {
            catAnimator.gameObject.SetActive(false);
            triggerCatAnimations.RestartSessions();
        }
        if (dogAnimator) {
            dogAnimator.gameObject.SetActive(false);
            triggerDogAnimations.RestartSessions();
        }
    }
}
