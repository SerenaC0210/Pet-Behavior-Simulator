using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CatPageManager : MonoBehaviour
{
    public GameObject background;
    public GameObject title;
    public GameObject carousel;
    public GameObject catInfoPanel;

    [Header("Main Menu Hint Text")]
    public GameObject exitHintText; // <-- NEW

    public TMP_Text catNameText;
    public TMP_Text catDescriptionText;

    public Image catImage1;
    public Image catImage2;

    [Header("Aggressive Cat")]
    public Sprite aggressiveCatSprite1;
    public Sprite aggressiveCatSprite2;

    [Header("Pleased Cat")]
    public Sprite pleasedCatSprite1;
    public Sprite pleasedCatSprite2;

    [Header("Scared Cat")]
    public Sprite scaredCatSprite1;
    public Sprite scaredCatSprite2;

    [TextArea] public string aggressiveDescription;
    [TextArea] public string pleasedDescription;
    [TextArea] public string scaredDescription;

    private void Start()
    {
        catInfoPanel.SetActive(false);
        exitHintText.SetActive(true); // show on main menu
    }

    private void ShowInfoPanel()
    {
        background.SetActive(false);
        title.SetActive(false);
        carousel.SetActive(false);

        exitHintText.SetActive(false); // hide when entering info panel
        catInfoPanel.SetActive(true);
    }

    public void ShowAggressiveCat()
    {
        ShowInfoPanel();

        catNameText.text = "Aggressive Cat";
        catDescriptionText.text = aggressiveDescription;

        catImage1.sprite = aggressiveCatSprite1;
        catImage2.sprite = aggressiveCatSprite2;
    }

    public void ShowPleasedCat()
    {
        ShowInfoPanel();

        catNameText.text = "Pleased Cat";
        catDescriptionText.text = pleasedDescription;

        catImage1.sprite = pleasedCatSprite1;
        catImage2.sprite = pleasedCatSprite2;
    }

    public void ShowScaredCat()
    {
        ShowInfoPanel();

        catNameText.text = "Scared Cat";
        catDescriptionText.text = scaredDescription;

        catImage1.sprite = scaredCatSprite1;
        catImage2.sprite = scaredCatSprite2;
    }

    public void BackToMenu()
    {
        catInfoPanel.SetActive(false);

        background.SetActive(true);
        title.SetActive(true);
        carousel.SetActive(true);

        exitHintText.SetActive(true); // show again on return
    }
}