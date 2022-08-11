using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action OnTakeDamage;
    public event Action OnDie;
    [SerializeField] private float maxHealth = 100f;
    public bool isInvulnerable;

    private float _health;

    private void Start()
    {
        _health = maxHealth;
    }

    public void DealDamage(float damage)
    {
        if (isInvulnerable) return;
        _health = Mathf.Max(_health - damage, 0f);
        OnTakeDamage?.Invoke();
        
        if (_health <= 0f)
        {
            Debug.Log("Die");
            OnDie?.Invoke();
            return;
        }
        

        print(_health);
    }

    public bool IsAlive() => _health > 0f;
}