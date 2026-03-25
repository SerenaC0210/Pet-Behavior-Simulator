using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [Tooltip("Time in seconds before this GameObject is destroyed.")]
    public float lifetime = 5f;

    void Start()
    {
        // Schedule the GameObject to be destroyed after 'lifetime' seconds
        Destroy(gameObject, lifetime);
    }
}
