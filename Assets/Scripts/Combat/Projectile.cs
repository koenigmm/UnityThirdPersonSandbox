using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float projectileVelocity = 100f;
    [SerializeField] private ParticleSystem impactVFXPrefab;
    private Rigidbody _rigidbody;

    private void Awake() => _rigidbody = GetComponent<Rigidbody>();

    private void OnEnable() => _rigidbody.velocity = transform.forward * projectileVelocity * Time.deltaTime;

    private void OnTriggerEnter(Collider other)
    {
        const float destroyTime = 2f;
        
        if (other.CompareTag("IgnoreRaycast")) return;
        print(other.name);
        
        if (other.TryGetComponent(out Health enemyHealth))
            enemyHealth.DealDamage(10f);

        if (impactVFXPrefab != null)
        {
            var vfxGameObject = Instantiate(impactVFXPrefab, other.transform.position, Quaternion.identity);
            Destroy(vfxGameObject, destroyTime);
        }
        
        Destroy(gameObject, 4f);
    }
}