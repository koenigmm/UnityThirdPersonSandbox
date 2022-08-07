using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
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
        
        print(_health);
    }

}
