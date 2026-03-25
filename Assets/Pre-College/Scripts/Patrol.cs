using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour
{
    [Header("Patrol Points")]
    [Tooltip("Waypoints for the agent to patrol between in order.")]
    [SerializeField] private Transform[] points;

    [Header("Agent Settings")]
    [Tooltip("Distance threshold at which the agent switches to the next point.")]
    [SerializeField] private float arrivalThreshold = 0.5f;

    private int _destPoint;
    private NavMeshAgent _agent;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.autoBraking = false;

        if (HasValidPoints())
            GotoNextPoint();
    }

    private void Update()
    {
        if (!HasValidPoints() || _agent.pathPending)
            return;

        if (_agent.remainingDistance < arrivalThreshold)
            GotoNextPoint();
    }

    private void GotoNextPoint()
    {
        if (!HasValidPoints())
            return;

        // Move agent to the next point and wrap around using modulo
        _agent.destination = points[_destPoint].position;
        _destPoint = (_destPoint + 1) % points.Length;
    }

    private bool HasValidPoints()
    {
        // Returns true if points array exists and has at least one valid Transform
        return points?.Length > 0;
    }

    public void SetDestination(Vector3 position)
    {
        _agent.SetDestination(position);
    }
}