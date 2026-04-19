using UnityEngine;

public class StartSessions : MonoBehaviour
{
    public Animator catAnimator;
    public Animator dogAnimator;
    public GameObject animalSelectUI;
    public bool isDog;
    public GameObject sessionCompleteUI;
    void Start()
    {
        animalSelectUI.SetActive(true);
        sessionCompleteUI.SetActive(false);
        if (catAnimator) catAnimator.gameObject.SetActive(false);
        if (dogAnimator) dogAnimator.gameObject.SetActive(false);
    }

    public void ChooseCat()
    {
        catAnimator.gameObject.SetActive(true);
        dogAnimator.gameObject.SetActive(false);
        animalSelectUI.SetActive(false);
    }

    public void ChooseDog()
    {
        dogAnimator.gameObject.SetActive(true);
        catAnimator.gameObject.SetActive(false);
        animalSelectUI.SetActive(false);
    }
    
    public void restart()
    {
        animalSelectUI.SetActive(true);
        if (catAnimator) catAnimator.gameObject.SetActive(false);
        if (dogAnimator) dogAnimator.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
