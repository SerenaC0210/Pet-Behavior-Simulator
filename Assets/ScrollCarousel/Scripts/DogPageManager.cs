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
    public Image dogImage;

    public Sprite aggressiveDogSprite;
    public Sprite pleasedDogSprite;
    public Sprite scaredDogSprite;

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
        dogImage.sprite = aggressiveDogSprite;
    }

    public void ShowPleasedDog()
    {
        ShowInfoPanel();
        dogNameText.text = "Pleased Dog";
        dogDescriptionText.text = pleasedDescription;
        dogImage.sprite = pleasedDogSprite;
    }

    public void ShowScaredDog()
    {
        ShowInfoPanel();
        dogNameText.text = "Scared Dog";
        dogDescriptionText.text = scaredDescription;
        dogImage.sprite = scaredDogSprite;
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