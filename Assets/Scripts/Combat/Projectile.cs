using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float projectileVelocity = 100f;
    [SerializeField] private ParticleSystem impactVFX;
    private void OnEnable()
    {
        GetComponent<Rigidbody>().velocity = Vector3.forward * projectileVelocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
