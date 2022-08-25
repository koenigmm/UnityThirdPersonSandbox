using System.Collections.Generic;
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
    [field: SerializeField] public Weapon CurrentWeapon { get; private set; }

    public bool isInInteractionArea;
    
    public Stamina PlayerStamina { get; private set; }
    public Transform MainCameraTransform { get; private set; }
    public ThirdPersonCameraController PlayerThirdPersonCameraController { get; private set; }

    public float PreviousDodgeTime { get; set; } = Mathf.NegativeInfinity;
    public LayerMask DefaultLayerMask;

    [SerializeField] private List<GameObject> meleeGameObjects;


    private void Awake()
    {
        PlayerStamina = GetComponent<Stamina>();
        PlayerThirdPersonCameraController = GetComponent<ThirdPersonCameraController>();
    }

    private void Start()
    {
        MainCameraTransform = Camera.main.transform;
        SwitchState(new PlayerFreeLookState(this));
    }

    private void OnEnable()
    {
        Health.OnDamage += HandleDamage;
        Health.OnDie += HandleDeath;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        Health.OnDamage -= HandleDamage;
        Health.OnDie -= HandleDeath;
    }

    private void HandleDamage() => SwitchState(new PlayerImpactState(this));

    private void HandleDeath() => SwitchState(new PlayerrDeadState(this));

    public void SetMeleeGameObjectsActive(bool isActive)
    {
        foreach (var meleeGameObject in meleeGameObjects)
        {
            meleeGameObject.SetActive(isActive);
        }
    }

    
}