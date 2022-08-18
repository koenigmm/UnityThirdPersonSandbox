using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action OnDamage;
    public event Action OnHealOrLevelUp;
    public event Action OnDie;
    public bool isInvulnerable;

    [SerializeField] private float levelUpRestorePercentage = 75f;
    private float _maxHealth;
    private float _health;
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
        _maxHealth = _level.GetMaxHealth();
        _health = _level.GetMaxHealth();
        OnHealOrLevelUp?.Invoke();
    }

    public void DealDamage(float damage)
    {
        if (isInvulnerable) return;
        _health = Mathf.Max(_health - damage, 0f);
        OnDamage?.Invoke();

        if (_health <= 0f)
        {
            Debug.Log("Die");
            OnDie?.Invoke();
            return;
        }
        print(_health);
    }

    public bool Heal(float healthPoints)
    {
        var healthBeforeHealing = _health;
        _health = MathF.Min(_maxHealth, healthPoints + _health);
        OnHealOrLevelUp?.Invoke();
        return healthBeforeHealing < _health;
    }

    public bool IsAlive() => _health > 0f;

    public float GetFraction() => _health / _maxHealth;
    
    private void HandleDeadlyVelocity() => DealDamage(_maxHealth);
    private void HandleLevelUp()
    {
        _maxHealth = _level.GetMaxHealth();
        _health = MathF.Max(_maxHealth * (levelUpRestorePercentage / 100f), _health);
        OnHealOrLevelUp?.Invoke();
    }
}