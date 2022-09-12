using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider), typeof(NavMeshAgent), typeof(Animator))]
[RequireComponent(typeof(Health))]

// Script execution order changed
public class EnemyStateMachine : StateMachine
{
    [field: SerializeField] public float AttackRange { get; private set; }
    [field: SerializeField] public SwordDamage Sword { get; private set; }
    [field: SerializeField] public float AttackDamage { get; private set; }
    [field: SerializeField] public float ChaseRange { get; private set; } = 10f;
    [field: SerializeField] public float ImpactOffset { get; private set; } = -2.5f;
    [field: SerializeField] public float StunningDelay { get; private set; } = 1f;
    [field: SerializeField] public float AttackDelay { get; private set; } = 0.9f;
    public Animator Animator { get; private set; }
    public GameObject Player { get; private set; }
    public Canvas HealthBarCanvas { get; private set; }
    public NavMeshAgent Agent { get; private set; }
    public Target Target { get; private set; }
    public Collider Collider { get; private set; }
    private Health _health;

    private void Awake()
    {
        Target = GetComponent<Target>();
        Agent = GetComponent<NavMeshAgent>();
        Collider = GetComponent<Collider>();
        HealthBarCanvas = GetComponentInChildren<Canvas>();
        Animator = GetComponent<Animator>();
        _health = GetComponent<Health>();
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        if (_health.IsAlive()) SwitchState(new EnemyIdleState(this));
        else SwitchState(new EnemyDeadState(this));
    }

    private void OnEnable()
    {
        _health.OnDamage += HandleDamage;
        _health.OnDie += HandleDeath;
    }

    private void OnDisable()
    {
        _health.OnDamage -= HandleDamage;
        _health.OnDie -= HandleDeath;
    }

    private void HandleDamage() => SwitchState(new EnemyImpactState(this));

    private void HandleDeath() => SwitchState(new EnemyDeadState(this));
}