using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    [SerializeField] private bool isAIControlled;
    private Collider _playerCollider;
    private List<Collider> _alreadyCollidedWith = new List<Collider>();
    private float _damage;

    private void OnEnable()
    {
        _alreadyCollidedWith.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isAIControlled && !other.CompareTag("Player")) return;
        if (!isAIControlled && !other.CompareTag("Enemy")) return; 
        
        if (_alreadyCollidedWith.Contains(other)) return;
        if (!other.TryGetComponent<Health>(out var health)) return;
        
        health.DealDamage(_damage);
        _alreadyCollidedWith.Add(other);
    }

    public void SetAttack(float damage)
    {
        _damage = damage;
    }
}