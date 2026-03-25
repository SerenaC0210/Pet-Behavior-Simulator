using UnityEngine;

public class RotateBackAndForth : MonoBehaviour
{
    public float rotationSpeed = 10f;
    public float rotationAngle = 45f;

    private float startingRotationY;

    void Start()
    {
        startingRotationY = transform.eulerAngles.y;
    }

    void Update()
    {
        float newYRotation = startingRotationY + Mathf.PingPong(Time.time * rotationSpeed, rotationAngle * 2) - rotationAngle;
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, newYRotation, transform.eulerAngles.z);
    }
}
