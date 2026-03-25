using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateUI : MonoBehaviour
{
    [Header("Controls UI")]
    [Tooltip("Dropdown for smooth snap turning options.")]
    public Dropdown smoothSnapDropdown;
    [Tooltip("Toggle for continuous movement.")]
    public Toggle continuousMoveToggle;
    [Tooltip("Toggle for teleportation movement.")]
    public Toggle teleportationToggle;

    [Header("Gameplay UI")]
    [Tooltip("Text displaying the player's score.")]
    public TextMeshProUGUI scoreText;
    [Tooltip("Text displaying the remaining time.")]
    public TextMeshProUGUI timeText;

    private XRControlsManagerPC _xrControlsManager;
    private ExperienceManager _experienceManager;

    private void Start()
    {
        _xrControlsManager = FindFirstObjectByType<XRControlsManagerPC>();
        if (!_xrControlsManager)
            Debug.LogError("Unable to find XRControlsManager.");

        _experienceManager = FindFirstObjectByType<ExperienceManager>();
        if (!_experienceManager)
            Debug.LogError("Unable to find ExperienceManager.");
        else
        {
            teleportationToggle.interactable = !_experienceManager.disableTeleportationToggle;
            smoothSnapDropdown.interactable = !_experienceManager.disableTurnDropdown;
            continuousMoveToggle.interactable = !_experienceManager.disableSmoothMotionToggle;
        }

        timeText.gameObject.SetActive(false);

        UpdateValues();
    }

    private void Update()
    {
        if (_experienceManager && _experienceManager.timerEnabled && !_experienceManager.GetIsGameWon())
        {
            int remaining = _experienceManager.GetTimeRemaining();
            timeText.SetText($"Time Left: {FormatTime(remaining)}");
        }
    }

    public void UpdateValues()
    {
        if (!_xrControlsManager) return;

        if (continuousMoveToggle)
            continuousMoveToggle.isOn = _xrControlsManager.smoothMotionEnabled;

        if (teleportationToggle)
            teleportationToggle.isOn = _xrControlsManager.teleportationEnabled;

        if (_experienceManager)
        {
            scoreText.SetText("Score: " + _experienceManager.GetCurrentScore());

            if (_experienceManager.timerEnabled)
                timeText.gameObject.SetActive(true);
        }

        SetDropdownValue();
    }

    private void SetDropdownValue()
    {
        if (smoothSnapDropdown)
            smoothSnapDropdown.value = _xrControlsManager.smoothTurnEnabled ? 1 : 0;
    }

    public static string FormatTime(int totalSeconds)
    {
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        return $"{minutes:00}:{seconds:00}";
    }
}
