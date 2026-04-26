using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DogPageManager : MonoBehaviour
{
    public GameObject background;
    public GameObject title;
    public GameObject carousel;
    public GameObject footer;
    public GameObject dogInfoPanel;

    public TMP_Text dogNameText;
    public TMP_Text dogDescriptionText;

    public Image dogImage1;
    public Image dogImage2;

    [Header("Aggressive Dog")]
    public Sprite aggressiveDogSprite1;
    public Sprite aggressiveDogSprite2;

    [Header("Pleased Dog")]
    public Sprite pleasedDogSprite1;
    public Sprite pleasedDogSprite2;

    [Header("Scared Dog")]
    public Sprite scaredDogSprite1;
    public Sprite scaredDogSprite2;

    [TextArea] public string aggressiveDescription;
    [TextArea] public string pleasedDescription;
    [TextArea] public string scaredDescription;

    private void Start()
    {
        dogInfoPanel.SetActive(false);
    }

    private void ShowInfoPanel()
    {
        background.SetActive(false);
        title.SetActive(false);
        carousel.SetActive(false);
        footer.SetActive(false);

        dogInfoPanel.SetActive(true);
    }

    public void ShowAggressiveDog()
    {
        ShowInfoPanel();

        dogNameText.text = "Aggressive Dog";
        dogDescriptionText.text = aggressiveDescription;

        dogImage1.sprite = aggressiveDogSprite1;
        dogImage2.sprite = aggressiveDogSprite2;
    }

    public void ShowPleasedDog()
    {
        ShowInfoPanel();

        dogNameText.text = "Pleased Dog";
        dogDescriptionText.text = pleasedDescription;

        dogImage1.sprite = pleasedDogSprite1;
        dogImage2.sprite = pleasedDogSprite2;
    }

    public void ShowScaredDog()
    {
        ShowInfoPanel();

        dogNameText.text = "Scared Dog";
        dogDescriptionText.text = scaredDescription;

        dogImage1.sprite = scaredDogSprite1;
        dogImage2.sprite = scaredDogSprite2;
    }

    public void BackToMenu()
    {
        dogInfoPanel.SetActive(false);

        background.SetActive(true);
        title.SetActive(true);
        carousel.SetActive(true);
        footer.SetActive(true);
    }
}