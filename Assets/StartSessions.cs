using UnityEngine;

public class StartSessions : MonoBehaviour
{
    public Animator catAnimator;
    public Animator dogAnimator;
    public GameObject animalSelectUI;
    public bool isDog;
    void Start()
    {
        animalSelectUI.SetActive(true);
        if (catAnimator) catAnimator.gameObject.SetActive(false);
        if (dogAnimator) dogAnimator.gameObject.SetActive(false);
    }

    public void ChooseCat()
    {
        isDog = false;
        catAnimator.gameObject.SetActive(true);
        if (dogAnimator) dogAnimator.gameObject.SetActive(false);
        animalSelectUI.SetActive(false);
    }

    public void ChooseDog()
    {
        isDog = true;
        dogAnimator.gameObject.SetActive(true);
        if (catAnimator) catAnimator.gameObject.SetActive(false);
        animalSelectUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
