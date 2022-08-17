using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    [field: SerializeField] public InputReader InputReader { get; private set; }
    [field: SerializeField] public CharacterController CharacterController { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public WeaponDamage Weapon { get; private set; }
    [field: SerializeField] public float FreeLookMovementSpeed { get; private set; }
    [field: SerializeField] public float TargetingMovementSpeed { get; private set; }
    [field: SerializeField] public float RotationDamping { get; private set; }
    [field: SerializeField] public Targeter Targeter { get; private set; }
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
    [field: SerializeField] public Health Health { get; private set; }
    [field: SerializeField] public Attack[] Attacks { get; private set; }
    [field: SerializeField] public float ImpactDistance { get; private set; } = -50f;
    [field: SerializeField] public float DodgeDuration { get; private set; } = 0.5f;
    [field: SerializeField] public float DodgeLength { get; private set; } = 2f;
    [field: SerializeField] public float JumpForce { get; private set; } = 2f;
    [field: SerializeField] public float JumpStaminaCost { get; private set; } = 33f;
    [field: SerializeField] public LedgeDetector LedgeDetector { get; private set; }
    [field: SerializeField] public float DodgeStaminaCost { get; private set; } = 25f;
    [field: SerializeField] public float BlockingStaminaCost { get; set; } = 10f;
    
    public Stamina PlayerStamina { get; private set; }
    public Transform MainCameraTransform { get; private set; }

    public float PreviousDodgeTime { get; set; } = Mathf.NegativeInfinity;


    private void Awake() => PlayerStamina = GetComponent<Stamina>();

    private void Start()
    {
        MainCameraTransform = Camera.main.transform;
        SwitchState(new PlayerFreeLookState(this));
    }

    private void OnEnable()
    {
        Health.OnHealthValueChange += HandleHealthValueChange;
        Health.OnDie += HandleDeath;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        Health.OnHealthValueChange -= HandleHealthValueChange;
        Health.OnDie -= HandleDeath;
    }

    private void HandleHealthValueChange() => SwitchState(new PlayerImpactState(this));

    private void HandleDeath() => SwitchState(new PlayerrDeadState(this));

    
}