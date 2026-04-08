using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CatPageManager : MonoBehaviour
{
    public TMP_Text catNameText;
    public TMP_Text catDescriptionText;
    public Image catImage;

    public Sprite aggressiveCatSprite;
    public Sprite pleasedCatSprite;
    public Sprite scaredCatSprite;

    [TextArea] public string aggressiveDescription;
    [TextArea] public string pleasedDescription;
    [TextArea] public string scaredDescription;

    public void ShowAggressiveCat()
    {
        catNameText.text = "Aggressive Cat";
        catDescriptionText.text = aggressiveDescription;
        catImage.sprite = aggressiveCatSprite;
    }

    public void ShowPleasedCat()
    {
        catNameText.text = "Pleased Cat";
        catDescriptionText.text = pleasedDescription;
        catImage.sprite = pleasedCatSprite;
    }

    public void ShowScaredCat()
    {
        catNameText.text = "Scared Cat";
        catDescriptionText.text = scaredDescription;
        catImage.sprite = scaredCatSprite;
    }
}