using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float projectileVelocity = 100f;
    [SerializeField] private ParticleSystem impactVFXPrefab;
    [SerializeField] private float damage;
    [SerializeField] private float maxFlightDuration = 4f;
    private Rigidbody _rigidbody;

    private void Awake() => _rigidbody = GetComponent<Rigidbody>();

    private void OnEnable() => _rigidbody.velocity = transform.forward * projectileVelocity * Time.deltaTime;

    private void Start() => Destroy(gameObject, maxFlightDuration);

    private void OnTriggerEnter(Collider other)
    {
        const float destroyTime = 2f;

        if (other.CompareTag("IgnoreRaycast")) return;
        print(other.name);
        ShowImpactEffect(other, destroyTime);

        if (other.TryGetComponent(out Health enemyHealth))
        {
            enemyHealth.DealDamage(damage);
            Destroy(gameObject);
        }
        
        

    }

    private void ShowImpactEffect(Collider other, float destroyTime)
    {
        if (impactVFXPrefab == null) return;
        var vfxGameObject = Instantiate(impactVFXPrefab, other.transform.position, Quaternion.identity);
        Destroy(vfxGameObject, destroyTime);
    }
}