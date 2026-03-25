using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class JellyIdleMovement : MonoBehaviour
{
    // Kept the name exactly as you used it.
    [SerializeField] private float thrust = 0.5f;

    private Rigidbody _rb;
    private bool _movementEnabled = true;
    private Coroutine _loop;

    // Internal tuning kept private & minimal
    private const float PulseTime = 1.2f;       // ramp up time
    private const float GlideTime = 0.5f;       // coast time
    private const float BrakeTime = 1.0f;       // active braking time (force opposite velocity)
    private const float IdleTime = 0.15f;       // small pause once fully stopped
    private const float BrakeK = 3.5f;          // braking “drag” strength (acceleration per second)
    private const float StopEpsilon = 0.03f;    // m/s threshold considered “stopped”
    private const float StopHoldTime = 0.2f;    // must remain under epsilon this long

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.MoveRotation(Random.rotation);

        _loop = StartCoroutine(SwimLoop());
    }

    private IEnumerator SwimLoop()
    {
        var waitFixed = new WaitForFixedUpdate();

        while (true)
        {
            // Pause the behavior while inside the net
            while (!_movementEnabled)
                yield return null;

            // --- 1) Accelerate (smooth ramp up) ---
            for (float t = 0f; t < PulseTime; t += Time.fixedDeltaTime)
            {
                if (!_movementEnabled) break;

                float u = Mathf.Clamp01(t / PulseTime);
                // SmoothStep easing for organic pulse
                float factor = u * u * (3f - 2f * u);

                _rb.AddForce(transform.up * (thrust * factor), ForceMode.Acceleration);
                yield return waitFixed;
            }
            if (!_movementEnabled) continue;

            // --- 2) Glide (no thrust; just coast) ---
            for (float t = 0f; t < GlideTime; t += Time.fixedDeltaTime)
            {
                if (!_movementEnabled) break;
                yield return waitFixed;
            }
            if (!_movementEnabled) continue;

            // --- 3) Brake (apply force opposite to current velocity) ---
            for (float t = 0f; t < BrakeTime; t += Time.fixedDeltaTime)
            {
                if (!_movementEnabled) break;

                Vector3 v = _rb.linearVelocity;
                if (v.sqrMagnitude > 1e-6f)
                {
                    // decel scales down over time so it eases in
                    float u = Mathf.Clamp01(t / BrakeTime);
                    float easeOut = 1f - (u * u * (3f - 2f * u)); // SmoothStep reversed
                    Vector3 brake = -v * (BrakeK * easeOut);      // constant-first where applicable
                    _rb.AddForce(brake, ForceMode.Acceleration);
                }
                yield return waitFixed;
            }
            if (!_movementEnabled) continue;

            // --- 4) Settle (must be REALLY stopped) ---
            float hold = 0f;
            while (true)
            {
                if (!_movementEnabled) break;

                Vector3 v = _rb.linearVelocity;
                // keep a gentle drag-like brake to finish the stop
                if (v.sqrMagnitude > 1e-6f)
                    _rb.AddForce(-v * BrakeK, ForceMode.Acceleration);

                if (v.sqrMagnitude <= StopEpsilon * StopEpsilon)
                {
                    hold += Time.fixedDeltaTime;
                    if (hold >= StopHoldTime)
                        break;
                }
                else
                {
                    hold = 0f;
                }

                yield return waitFixed;
            }
            if (!_movementEnabled) continue;

            // Clamp any sub-precision jitter so we’re truly at rest
            //_rb.linearVelocity = Vector3.zero;
            //_rb.angularVelocity = Vector3.zero;

            // --- 5) Idle briefly, then rotate for the next swim ---
            for (float t = 0f; t < IdleTime; t += Time.fixedDeltaTime)
            {
                if (!_movementEnabled) break;
                yield return waitFixed;
            }
            if (_movementEnabled)
                _rb.MoveRotation(Random.rotation);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Jelly Fish Net"))
        {
            _movementEnabled = false;
            _rb.linearVelocity = Vector3.zero;        // stop immediately when caught
            _rb.angularVelocity = Vector3.zero;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Jelly Fish Net"))
        {
            _movementEnabled = true;            // coroutine naturally resumes on next loop
        }
    }
    
    private void OnDestroy()
    {
        if (_loop != null)
        {
            StopCoroutine(_loop);
            _loop = null;
        }
    }

}
