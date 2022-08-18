using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class LevelUpZone : MonoBehaviour
{
    
    private ParticleSystem _particleSystem;
    private PlayerStateMachine _stateMachine;
    private bool _canShowUI;

    private void Awake()
    {
        _particleSystem = GetComponentInChildren<ParticleSystem>();
        GetComponent<Collider>().isTrigger = true;
    }

    private void Start()
    {
        _particleSystem.Stop();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        _canShowUI = other.TryGetComponent(out _stateMachine);
        if (!_canShowUI) return;
        HandleCampfireZone(other);
    }
    

    private void HandleCampfireZone(Component other)
    {
        if (_canShowUI)
        {
            _stateMachine.isInInteractionArea = true;
            _particleSystem.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_canShowUI)
        {
            _stateMachine.isInInteractionArea = false;
            _stateMachine = null;
            _particleSystem.Stop();
        }

        _canShowUI = false;
    }
}