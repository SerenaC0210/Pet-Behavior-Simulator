using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class MenuRoom : MonoBehaviour
{
    private ExperienceManager _experienceManager;

    public Transform menuSpawnPoint;
    
    [Header("UI Elements")] 
    [Tooltip("UI Text for the title of the experience.")]
    public TextMeshProUGUI experienceTitle;
    [Tooltip("UI Text for the instructions for the experience.")]
    public TextMeshProUGUI instructionsText;
    [Tooltip("UI Text for the win/lose message.")]
    public TextMeshProUGUI winLoseText;
    [Tooltip("GameObject that groups the score and time UI texts.")]
    public GameObject scoreAndTimeGroup;
    [Tooltip("UI Text for the score.")]
    public TextMeshProUGUI scoreText;
    [Tooltip("UI Text for the time left.")]
    public TextMeshProUGUI timeText;
    [Tooltip("UI Text for the start button.")]
    public TextMeshProUGUI startButtonText;

    [Header("Room Material Settings")]
    [Tooltip("The material displayed on the menu room at the start of the experience.")]
    public Material startMaterial;
    [Tooltip("The material displayed on the menu room at the end of the experience.")]
    public Material endMaterial;
    public MeshRenderer roomMeshRenderer;
    
    public void Initialize(ExperienceManager manager)
    {
        _experienceManager = manager;
    }

    public void StartSetUp()
    {
        if (!_experienceManager)
        {
            Debug.LogError("MenuRoom.StartSetUp called with null ExperienceManager");
            return;
        }

        experienceTitle.SetText(_experienceManager.experienceTitle);
        instructionsText.SetText(_experienceManager.instructionsText);
        winLoseText.gameObject.SetActive(false);
        scoreAndTimeGroup.SetActive(false);
        startButtonText.SetText(_experienceManager.startButtonString);
        roomMeshRenderer.material = startMaterial;
    }

    public void EndSetUp()
    {
        experienceTitle.SetText(_experienceManager.experienceTitle);
        
        if (_experienceManager)
        {
            // Instructions Text
            instructionsText.gameObject.SetActive(false);
            
            // Win/Lose Text
            if (_experienceManager.GetIsGameWon())
                winLoseText.SetText(_experienceManager.winText);
            else
                winLoseText.SetText(_experienceManager.loseText);
            
            winLoseText.gameObject.SetActive(true);
            
            // Score
            scoreText.gameObject.SetActive(true);
            scoreText.SetText("Score: " + _experienceManager.GetCurrentScore());
            
            // Timer
            if (_experienceManager.timerEnabled)
            {
                timeText.gameObject.SetActive(true);
                timeText.SetText("Time Left: " + UpdateUI.FormatTime(_experienceManager.GetTimeRemaining()));
            }
            else
            {
                timeText.gameObject.SetActive(false);
            }
            
            scoreAndTimeGroup.SetActive(true);
            
            startButtonText.SetText(_experienceManager.restartButtonString);
        }
        
        roomMeshRenderer.material = endMaterial;
    }

    public void StartButtonPressed()
    {
        _experienceManager.StartButtonPressed();
    }
}
