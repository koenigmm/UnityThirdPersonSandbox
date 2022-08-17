using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : StateMachine
{
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public GameObject Player { get; private set; }
    [field: SerializeField] public float AttackRange { get; private set; }
    [field: SerializeField] public WeaponDamage Weapon { get; private set; }
    [field: SerializeField] public float AttackDamage { get; private set; }
    [field: SerializeField] public Health Health { get; private set; }
    [field: SerializeField] public float ChaseRange { get; private set; } = 10f;
    [field: SerializeField] public float ImpactOffset { get; private set; } = -2.5f;
    [field: SerializeField] public float StunningDelay { get; private set; } = 1f;
    [field: SerializeField] public float AttackDelay { get; private set; } = 0.9f;
    public NavMeshAgent Agent { get; private set; }
    public Target Target { get; private set; }
    public Collider Collider { get; private set; }

    private void OnEnable()
    {
        Health.OnDamage += HandleDamage;
        Health.OnDie += HandleDeath;
    }

    private void OnDisable()
    {
        Health.OnDamage -= HandleDamage;
        Health.OnDie -= HandleDeath;
    }

    private void HandleDamage() => SwitchState(new EnemyImpactState(this));

    private void HandleDeath() => SwitchState(new EnemyDeadState(this));

    private void Awake()
    {
        Target = GetComponent<Target>();
        Agent = GetComponent<NavMeshAgent>();
        Collider = GetComponent<Collider>();
    }

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        SwitchState(new EnemyIdleState(this));
    }
}