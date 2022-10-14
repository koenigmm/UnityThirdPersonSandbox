using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerStateMachine : StateMachine
{
    [field: SerializeField] public SwordDamage Sword { get; private set; }
    [field: SerializeField] public float FreeLookMovementSpeed { get; private set; }
    [field: SerializeField] public float TargetingMovementSpeed { get; private set; }
    [field: SerializeField] public float RotationDamping { get; private set; }
    [field: SerializeField] public Targeter Targeter { get; private set; }
    [field: SerializeField] public Attack[] Attacks { get; private set; }
    [field: SerializeField] public float ImpactDistance { get; private set; } = -50f;
    [field: SerializeField] public float ImpactDuration { get; private set; } = 0.34f;
    [field: SerializeField] public float DodgeDuration { get; private set; } = 0.5f;
    [field: SerializeField] public float DodgeLength { get; private set; } = 2f;
    [field: SerializeField] public float JumpForce { get; private set; } = 2f;
    [field: SerializeField] public float JumpStaminaCost { get; private set; } = 33f;
    [field: SerializeField] public LedgeDetector LedgeDetector { get; private set; }
    [field: SerializeField] public float DodgeStaminaCost { get; private set; } = 25f;
    [field: SerializeField] public float BlockingStaminaCost { get; set; } = 10f;
    [field: SerializeField] public RangedWeapon CurrentRangedWeapon { get; private set; }
    [field: SerializeField] public bool ShouldHideSwordInFreeLookState { get; private set; } = true;

    // public bool isInInteractionArea;
    [SerializeField] private bool ShouldBeStunnedByRangedAttacks  = false;
    [SerializeField] private List<GameObject> meleeGameObjects;
    [SerializeField] private List<MeshRenderer> rangedWeaponMeshRenderers;

    public InputReader InputReader { get; private set; }
    public Health Health { get; private set; }
    public ForceReceiver ForceReceiver { get; private set; }
    public Animator Animator { get; private set; }
    public Stamina PlayerStamina { get; private set; }
    public Transform MainCameraTransform { get; private set; }
    public ThirdPersonCameraController PlayerThirdPersonCameraController { get; private set; }
    public CharacterController CharacterController { get; private set; }
    public bool IsControllable { get; private set; } = true;


    private void Awake()
    {
        PlayerStamina = GetComponent<Stamina>();
        PlayerThirdPersonCameraController = GetComponent<ThirdPersonCameraController>();
        InputReader = GetComponent<InputReader>();
        Health = GetComponent<Health>();
        ForceReceiver = GetComponent<ForceReceiver>();
        Animator = GetComponent<Animator>();
        CharacterController = GetComponent<CharacterController>();
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

    private void HandleDamage(bool rangedDamage)
    {
        if (rangedDamage && !ShouldBeStunnedByRangedAttacks) return;
        SwitchState(new PlayerImpactState(this));
    }

    private void HandleDeath() => SwitchState(new PlayerDeadState(this));

    public void SetMeleeGameObjectsActive(bool isActive)
    {
        foreach (var meleeGameObject in meleeGameObjects)
        {
            meleeGameObject.SetActive(isActive);
        }
    }

    public void SetCurrentRangedWeaponActive(bool isActive)
    {
        foreach (var meshRenderer in rangedWeaponMeshRenderers)
        {
            meshRenderer.enabled = isActive;
        }
    }

    public void SetPlayerControlsActive(bool isActive)
    {
        IsControllable = isActive;
        Cursor.lockState = isActive ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !isActive;
        ForceReceiver.enabled = isActive;
        Animator.enabled = isActive;
        PlayerThirdPersonCameraController.enabled = isActive;
    }
}