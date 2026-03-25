using UnityEngine;

public class EnemyNPC : MonoBehaviour
{
    private ExperienceManager _experienceManager;
    void Start()
    {
        _experienceManager = FindFirstObjectByType<ExperienceManager>();
        if (!_experienceManager)
            Debug.LogError("Experience Manager not found.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _experienceManager.Lose();
        }
    }
}
