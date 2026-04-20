using UnityEngine;

public class StartSessions : MonoBehaviour
{
    public Animator catAnimator;
    public Animator dogAnimator;
    public GameObject animalSelectUI;
    public bool isDog;
    public GameObject catSessionUI;
    void Start()
    {
        animalSelectUI.SetActive(true);
        catSessionUI.SetActive(false);
        if (catAnimator) catAnimator.gameObject.SetActive(false);
        if (dogAnimator) dogAnimator.gameObject.SetActive(false);
    }

    public void ChooseCat()
    {
        catAnimator.gameObject.SetActive(true);
        dogAnimator.gameObject.SetActive(false);
        animalSelectUI.SetActive(false);
        catSessionUI.SetActive(true);
    }

    public void ChooseDog()
    {
        dogAnimator.gameObject.SetActive(true);
        catAnimator.gameObject.SetActive(false);
        animalSelectUI.SetActive(false);
        catSessionUI.SetActive(false);
    }
    
    public void restart()
    {
        animalSelectUI.SetActive(true);
        catSessionUI.SetActive(false);
        if (catAnimator) catAnimator.gameObject.SetActive(false);
        if (dogAnimator) dogAnimator.gameObject.SetActive(false);
    }
}
