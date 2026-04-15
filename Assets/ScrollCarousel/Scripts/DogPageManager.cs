using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DogPageManager : MonoBehaviour
{
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

    public void ShowAggressiveDog()
    {
        dogInfoPanel.SetActive(true);
        dogNameText.text = "Aggressive Dog";
        dogDescriptionText.text = aggressiveDescription;
        dogImage.sprite = aggressiveDogSprite;
    }

    public void ShowPleasedDog()
    {
        dogInfoPanel.SetActive(true);
        dogNameText.text = "Pleased Dog";
        dogDescriptionText.text = pleasedDescription;
        dogImage.sprite = pleasedDogSprite;
    }

    public void ShowScaredDog()
    {
        dogInfoPanel.SetActive(true);
        dogNameText.text = "Scared Dog";
        dogDescriptionText.text = scaredDescription;
        dogImage.sprite = scaredDogSprite;
    }

    public void ClosePanel()
    {
        dogInfoPanel.SetActive(false);
    }
}