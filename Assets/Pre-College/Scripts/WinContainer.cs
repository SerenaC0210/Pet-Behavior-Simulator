using UnityEngine;

public class WinContainer : MonoBehaviour
{
    private ExperienceManager _experienceManager;
    public ParticleSystem sparks;
    
    private void Start()
    {
        _experienceManager = FindFirstObjectByType<ExperienceManager>();
        if (!_experienceManager)
        {
            Debug.LogError("ExperienceManager not found.");
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        WinObject wo = other.gameObject.GetComponent<WinObject>();
        if (wo)
        {
            if (sparks)
                sparks.Play();
            _experienceManager.AddScore(wo.scoreAmount);
            wo.OnEnterContainer();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        WinObject wo = other.gameObject.GetComponent<WinObject>();
        if (wo)
        {
            _experienceManager.SubtractScore(wo.scoreAmount);
        }
    }
}
