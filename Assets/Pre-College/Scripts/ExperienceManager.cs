using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ExperienceManager : MonoBehaviour
{
    [Tooltip("Spawns player directly in the experience rather than Menu, and disables winning/losing.")]
    public bool sandboxMode = false;

    [Header("Scoring")]
    [Tooltip("The player's current score.")]
    [SerializeField] private int currentScore;
    [Tooltip("Score the player starts with.")]
    public int startingScore = 0;
    [Tooltip("Score needed in order to win.")]
    public int scoreNeeded = 5;
    [Tooltip("Win once the score reaches the Score Needed amount.")]
    public bool winAtReachScoreNeeded = true;

    [Header("Timer Settings")]
    [Tooltip("Determines if the experience uses a timer.")]
    public bool timerEnabled = false;
    [Tooltip("The number of seconds until the experience ends.")]
    [Range(1, 3599)]
    public int seconds = 3599;

    [Header("Main Menu UI Elements")]
    [Tooltip("The name of your experience.")]
    public string experienceTitle = "Experience Title";
    [Tooltip("Instructions for your experience.")] 
    [TextArea]
    public string instructionsText = "Instructions";
    [Tooltip("Text displayed on the start button.")]
    public string startButtonString = "Start Experience";
    [Tooltip("Text displayed on the restart button.")]
    public string restartButtonString = "Restart Experience";
    [Tooltip("Text to display when the player wins.")]
    [TextArea]
    public string winText = "You win!";
    [Tooltip("Text to display when the player loses.")]
    [TextArea]
    public string loseText = "You lose!";

    [Header("Spawn Point")]
    [Tooltip("Where the player will spawn once the experience starts.")]
    public Transform spawnPoint;

    [Header("Disable Toggling Controls")]
    public bool disableTeleportationToggle = false;
    public bool disableSmoothMotionToggle = false;
    public bool disableTurnDropdown = false;
    
    private MenuRoom _menuRoom;
    private XRControlsManagerPC _xrControlsManager;
    private bool _isGameOver = false;
    private bool _isGameWon = false;
    private Coroutine _timerRoutine;
    private int _timeRemaining;

    private void Start()
    {
        currentScore = startingScore;

        _menuRoom = FindFirstObjectByType<MenuRoom>();
        if (!_menuRoom)
            Debug.LogError("Menu Room not found. Please ensure that the Menu Room prefab is placed in the scene.");
        else
            _menuRoom.Initialize(this);

        _xrControlsManager = FindFirstObjectByType<XRControlsManagerPC>();
        if (!_xrControlsManager)
        {
            Debug.LogError("XR Controls Manager could not be found.");
        }
        else
        {
            if (sandboxMode)
                _xrControlsManager.disableMovementOnStart = false;
            else
                _xrControlsManager.StartSetUp();
        }

        if (!sandboxMode)
            StartSetup();
    }

    private void StartSetup()
    {
        _menuRoom.StartSetUp();
        TeleportPlayer(_menuRoom.menuSpawnPoint.transform, disableControls: true, restoreDefaultControls: false);
    }

    private void TeleportPlayer(Transform spawnPointTransform, bool disableControls = false, bool restoreDefaultControls = false)
    {
        if (!_xrControlsManager) return;

        Transform playerTransform = _xrControlsManager.gameObject.transform;
        playerTransform.position = spawnPointTransform.position;
        playerTransform.rotation = spawnPointTransform.rotation;

        if (restoreDefaultControls)
            _xrControlsManager.RestoreDefaults();

        if (disableControls)
            _xrControlsManager.DisableAllControls();
    }

    public void StartButtonPressed()
    {
        if (_isGameOver)
        {
            ReloadScene();
            return;
        }

        TeleportPlayer(spawnPoint, restoreDefaultControls: true);

        if (timerEnabled && !sandboxMode)
        {
            _timeRemaining = seconds;
            _timerRoutine = StartCoroutine(TimerCountdown());
        }
    }

    public void AddScore(int amount)
    {
        if (_isGameOver) return;

        currentScore += amount;
        _xrControlsManager.UpdateValues();
        WinCheck();
    }

    public void SubtractScore(int amount)
    {
        if (_isGameOver) return;

        currentScore -= amount;
        _xrControlsManager.UpdateValues();
        WinCheck();
    }

    private void WinCheck()
    {
        if (currentScore < scoreNeeded)
        {
            _isGameWon = false;
            return;
        }

        if (winAtReachScoreNeeded)
            Win();
    }

    private void Win()
    {
        StopTimer();
        if (sandboxMode) return;

        _isGameWon = true;
        _isGameOver = true;
        End();
    }

    public void Lose()
    {
        StopTimer();
        if (sandboxMode) return;

        _isGameWon = false;
        _isGameOver = true;
        End();
    }

    private void End()
    {
        _menuRoom.EndSetUp();
        TeleportPlayer(_menuRoom.menuSpawnPoint.transform, disableControls: true);
        if (_xrControlsManager.wristWatchUI)
            _xrControlsManager.wristWatchUI.SetActive(false);
        SetGrabbablesActive(false);
    }

    public bool GetIsGameWon() => _isGameWon;
    public int GetCurrentScore() => currentScore;
    public int GetTimeRemaining() => _timeRemaining;

    private IEnumerator TimerCountdown()
    {
        while (_timeRemaining > 0)
        {
            yield return new WaitForSeconds(1f);
            _timeRemaining--;

            if (_timeRemaining <= 0)
            {
                winAtReachScoreNeeded = true; //hacky, but works for now.
                WinCheck();
                if (!_isGameWon)
                    Lose();
            }
        }
    }

    private void StopTimer()
    {
        if (_timerRoutine != null)
        {
            StopCoroutine(_timerRoutine);
            _timerRoutine = null;
        }
    }

    private static void ReloadScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    private static void SetGrabbablesActive(bool state)
    {
        var grabbableObjects = FindObjectsByType<XRGrabInteractable>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var child in grabbableObjects)
            child.gameObject.SetActive(state);
    }
}
