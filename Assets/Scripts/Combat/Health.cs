using System;
using SavingSystem;
using UnityEngine;

public class Health : SaveableEntity
{
    public event Action OnDamage;
    public event Action OnHeal;
    public event Action OnDie;
    public bool isInvulnerable;

    [SerializeField] private float levelUpRestorePercentage = 75f;
    public float MaxHealth { get; private set; }
    public float CurrentHealth { get; private set; }
    private ForceReceiver _forceReceiver;
    private Level _level;
    private bool _isRestoredFromSavegame;

    private void OnEnable()
    {
        if (!gameObject.CompareTag("Player")) return;
        _forceReceiver.OnDeadlyVelocity += HandleDeadlyVelocity;
        _level.OnHealthLevelUp += HandleLevelUp;
    }


    private void OnDisable()
    {
        if (!gameObject.CompareTag("Player")) return;
        _forceReceiver.OnDeadlyVelocity -= HandleDeadlyVelocity;
        _level.OnHealthLevelUp -= HandleLevelUp;
    }

    private void Awake()
    {
        _forceReceiver = GetComponent<ForceReceiver>();
        _level = GetComponent<Level>();
    }

    private void Start()
    {
        MaxHealth = _level.GetMaxHealth();
        if (!_isRestoredFromSavegame)CurrentHealth = _level.GetMaxHealth();
        OnHeal?.Invoke();
    }

#if UNITY_EDITOR
    private void Update()
    {
        SetGuid();
    }
#endif


    public void DealDamage(float damage)
    {
        if (isInvulnerable) return;
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0f);
        OnDamage?.Invoke();

        if (CurrentHealth <= 0f)
        {
            Debug.Log("Die");
            OnDie?.Invoke();
            return;
        }

        print(CurrentHealth);
    }

    public bool Heal(float healthPoints)
    {
        var healthBeforeHealing = CurrentHealth;
        CurrentHealth = MathF.Min(MaxHealth, healthPoints + CurrentHealth);
        OnHeal?.Invoke();
        return healthBeforeHealing < CurrentHealth;
    }

    public bool IsAlive() => CurrentHealth > 0f;

    public float GetFraction() => CurrentHealth / MaxHealth;

    private void HandleDeadlyVelocity() => DealDamage(MaxHealth);

    private void HandleLevelUp()
    {
        MaxHealth = _level.GetMaxHealth();
        CurrentHealth = MathF.Max(MaxHealth * (levelUpRestorePercentage / 100f), CurrentHealth);
        OnHeal?.Invoke();
    }

    public override void PopulateSaveData(SaveData saveData)
    {
        var healthData = new FloatWithID
        {
            uuid = uuid,
            savedFloat = CurrentHealth
        };

        saveData.healthComponents.Add(healthData);
    }

    public override void LoadFromSaveData(SaveData saveData)
    {
        foreach (var healthComponent in saveData.healthComponents)
        {
            if (healthComponent.uuid != uuid) continue;
            CurrentHealth = healthComponent.savedFloat;
            _isRestoredFromSavegame = true;
        }
        
    }
}