using System;
using UnityEngine;
using UnityEngine.AI;

public class TestAI : MonoBehaviour
{
    [SerializeField] private float _chaseRange = 10f;
    private NavMeshAgent _agent;
    private GameObject _player;
    
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        var distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);

        if (distanceToPlayer < _chaseRange)
        {
            _agent.isStopped = false;
            _agent.destination = _player.transform.position;
        }
        else
        {
            _agent.isStopped = true;
            _agent.ResetPath();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _chaseRange);
    }
}
