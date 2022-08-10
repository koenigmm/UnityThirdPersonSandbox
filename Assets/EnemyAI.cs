using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float impactOffset = -10f;
    private NavMeshAgent _agent;
    private const float ChaseRange = 10f;
    private GameObject _player;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindWithTag("Player");
    }
    
    private void Update()
    {
        if (!IsInChaseRange()) return;
        
        _agent.enabled = true;
        _agent.SetDestination(_player.transform.position);
    }

    public bool IsInChaseRange()
    {
        return Vector3.Distance(transform.position, _player.transform.position) < ChaseRange;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ChaseRange);
    }

    public void HandleImpact()
    {
        _agent.isStopped = true;
        transform.Translate(0, 0, impactOffset, Space.Self);
    }

    public void ActivateAI()
    {
        _agent.isStopped = false;
    }
    
}