using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

[RequireComponent(typeof(Collider), typeof(NavMeshAgent), typeof(Animator))]
[RequireComponent(typeof(Health))]

// Script execution order changed
public class EnemyStateMachine : StateMachine
{
    public bool isAlarmed; 
    // Combat
    [field: SerializeField] public float AttackRange { get; private set; }
    [field: SerializeField] public float AlarmRange { get; private set; } = 10f;
    [field: SerializeField] public SwordDamage Sword { get; private set; }
    [field: SerializeField] public float AttackDamage { get; private set; }
    [field: SerializeField] public float ImpactOffset { get; private set; } = -2.5f;
    [field: SerializeField] public float AttackDelay { get; private set; } = 0.9f;
    
    // AI Behaviour
    [field: SerializeField] public float ChaseRange { get; private set; } = 10f;
    [field: SerializeField] public float SuspiciousTime { get; private set; } = 5f;
    [field: SerializeField] public float StunningDelay { get; private set; } = 1f;
    
    // Ranged Attacks
    [field: SerializeField] public bool IsMelee { get; private set; } = true;
    [field: SerializeField] public GameObject Projectile { get; private set; }
    [field: SerializeField] public Transform ProjectileLaunchPoint { get; private set; }
    [field: SerializeField] public float TimeBetweenRangedAttacks { get; private set; } = 1.2f;
    [field: SerializeField] public VisualEffect MuzzleFlashVFX{ get; private set; }
    
    // Waypoints
    [field: SerializeField] public Waypoint Waypoints { get; private set; }
    [field: SerializeField] public float ChasingSpeed { get; private set; }
    [field: SerializeField] public float WalkingSpeed { get; private set; }
    [field: SerializeField] public float WaypointDwellingTime{ get; private set; } = 2.5f;

    public Animator Animator { get; private set; }
    public GameObject Player { get; private set; }
    public Canvas HealthBarCanvas { get; private set; }
    public NavMeshAgent Agent { get; private set; }
    public Target Target { get; private set; }
    public Collider Collider { get; private set; }
    public Vector3 DefaultPosition { get; set; }
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, ChaseRange);
    }

    private void Start()
    { 
        if (DefaultPosition == Vector3.zero) DefaultPosition = transform.position;
        if (_health.IsAlive()) SwitchState(new EnemyIdleState(this));
        else SwitchState(new EnemyDeadState(this));

        if (MuzzleFlashVFX != null) MuzzleFlashVFX.enabled = false;
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

    private void HandleDamage(bool rangedDamage) => SwitchState(new EnemyImpactState(this));

    private void HandleDeath() => SwitchState(new EnemyDeadState(this));
}