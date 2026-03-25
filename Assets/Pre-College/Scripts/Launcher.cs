using System.Collections;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The origin point from which objects will be launched.")]
    public GameObject origin;

    [Tooltip("The prefab object to launch.")]
    public GameObject launchObject;

    [Header("Settings")]
    [Tooltip("Force applied when launching the object.")]
    public float launchForce = 10f;

    [Tooltip("Time interval between launches, in seconds.")]
    public float launchInterval = 2f;

    private GameObject _currentLaunchObject;
    private Rigidbody _objectRb;
    private bool _isRunning;

    private void Update()
    {
        if (_isRunning)
        {
            return;
        }

        StartCoroutine(Launch());
    }

    private IEnumerator Launch()
    {
        _isRunning = true;
        yield return new WaitForSeconds(launchInterval);

        if (launchObject && origin)
        {
            _currentLaunchObject = Instantiate(launchObject);
            _currentLaunchObject.transform.position = origin.transform.position;

            if (_currentLaunchObject.TryGetComponent(out _objectRb))
            {
                _objectRb.AddForce(launchForce * origin.transform.up, ForceMode.Force);
            }
        }

        _isRunning = false;
    }
}