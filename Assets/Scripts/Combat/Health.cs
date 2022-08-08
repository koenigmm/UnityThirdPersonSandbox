using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action OnTakeDamage;
    public event Action OnDie;
    [SerializeField] private float maxHealth = 100f;

    private float _health;
    
    private void Start()
    {
        _health = maxHealth;
    }

    public void DealDamage(float damage)
    {
        if (_health <= 0f) return;

        _health = Mathf.Max(_health - damage, 0f);
        OnTakeDamage?.Invoke();
        
        if (_health <= 0f)
            OnDie?.Invoke();
        
        print(_health);
    }

}
