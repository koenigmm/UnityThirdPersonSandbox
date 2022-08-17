using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action OnHealthValueChange;
    public event Action OnLevelUp;
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
    }

    public void DealDamage(float damage)
    {
        if (isInvulnerable) return;
        _health = Mathf.Max(_health - damage, 0f);
        OnHealthValueChange?.Invoke();

        if (_health <= 0f)
        {
            Debug.Log("Die");
            OnDie?.Invoke();
            return;
        }
        print(_health);
    }

    public bool IsAlive() => _health > 0f;

    public float GetFraction() => _health / _maxHealth;
    
    private void HandleDeadlyVelocity() => DealDamage(_maxHealth);
    private void HandleLevelUp()
    {
        _maxHealth = _level.GetMaxHealth();
        _health = _maxHealth * (levelUpRestorePercentage / 100f);
        OnLevelUp?.Invoke();
    }
}