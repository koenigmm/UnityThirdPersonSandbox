using System;
using UnityEngine;

public class Health : MonoBehaviour
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
        CurrentHealth = _level.GetMaxHealth();
        OnHeal?.Invoke();
    }

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
}