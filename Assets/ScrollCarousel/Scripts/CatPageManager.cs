using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CatPageManager : MonoBehaviour
{
    public GameObject background;
    public GameObject title;
    public GameObject carousel;
    public GameObject footer;
    public GameObject catInfoPanel;

    public TMP_Text catNameText;
    public TMP_Text catDescriptionText;
    public Image catImage;

    public Sprite aggressiveCatSprite;
    public Sprite pleasedCatSprite;
    public Sprite scaredCatSprite;

    [TextArea] public string aggressiveDescription;
    [TextArea] public string pleasedDescription;
    [TextArea] public string scaredDescription;

    private void Start()
    {
        catInfoPanel.SetActive(false);
    }

    private void ShowInfoPanel()
    {
        background.SetActive(false);
        title.SetActive(false);
        carousel.SetActive(false);
        footer.SetActive(false);

        catInfoPanel.SetActive(true);
    }

    public void ShowAggressiveCat()
    {
        ShowInfoPanel();
        catNameText.text = "Aggressive Cat";
        catDescriptionText.text = aggressiveDescription;
        catImage.sprite = aggressiveCatSprite;
    }

    public void ShowPleasedCat()
    {
        ShowInfoPanel();
        catNameText.text = "Pleased Cat";
        catDescriptionText.text = pleasedDescription;
        catImage.sprite = pleasedCatSprite;
    }

    public void ShowScaredCat()
    {
        ShowInfoPanel();
        catNameText.text = "Scared Cat";
        catDescriptionText.text = scaredDescription;
        catImage.sprite = scaredCatSprite;
    }

    public void BackToMenu()
    {
        catInfoPanel.SetActive(false);

        background.SetActive(true);
        title.SetActive(true);
        carousel.SetActive(true);
        footer.SetActive(true);
    }
}