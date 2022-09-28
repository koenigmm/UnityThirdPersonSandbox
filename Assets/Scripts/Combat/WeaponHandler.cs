using System;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private Collider weaponCollider;
    [SerializeField] private Health _health;
    private bool _canFight = true;

    private void Awake()
    {
        if (_health == null) _health = GetComponent<Health>();
    }

    private void OnEnable() => _health.OnDie += HandleDeath;

    private void OnDisable() => _health.OnDie -= HandleDeath;

    // Animation Event
    public void EnableWeapon()
    {
        if (_canFight) weaponCollider.enabled = true;
    }

    // Animation Event
    public void DisableWeapon ()
    {
        if (_canFight) weaponCollider.enabled = false;
    }
    
    private void HandleDeath()
    {
        _canFight = false;
        DisableWeapon();
        enabled = false;
    }
}
