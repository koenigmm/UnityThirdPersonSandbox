using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class HealingZone : MonoBehaviour
{
    [SerializeField] private float amountOfHealthPoints = 100f;
    [SerializeField] private float healthPerInterval = 5f;
    [SerializeField] private int interval = 1;
    [SerializeField] private bool canRespawn;
    [SerializeField] private float respawnTime = 60f;
    private float _healthPointsInZone;
    private PlayerStateMachine _stateMachine;
    private float _timer;
    private bool _canUsePlayerStateMachine;
    private bool _isUsedAndEmpty;
    private Collider _collider;
    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _meshRenderer = GetComponentInChildren<MeshRenderer>();

        // For Saving System
        if (_isUsedAndEmpty)
            DeactivateHealingZone();
    }

    private void Start()
    {
        _healthPointsInZone = amountOfHealthPoints;
    }

    private void Update()
    {
        if (!_canUsePlayerStateMachine) return;

        if (_timer >= interval && _healthPointsInZone > 0f)
            HandleHealingZone();

        _timer += Time.deltaTime;
    }


    private void OnTriggerEnter(Collider other)
    {
        _timer = 0f;
        _canUsePlayerStateMachine = other.TryGetComponent(out _stateMachine);
    }

    private void OnTriggerExit(Collider other)
    {
        _stateMachine = null;
        _canUsePlayerStateMachine = false;
    }


    private void HandleHealingZone()
    {
        _timer = 0f;
        var canHeal = _stateMachine.Health.Heal(healthPerInterval);

        if (canHeal)
            _healthPointsInZone -= healthPerInterval;

        if (Mathf.Approximately(0f, _healthPointsInZone)) 
            DeactivateHealingZone();
    }

    private void DeactivateHealingZone()
    {
        _collider.enabled = false;
        _meshRenderer.enabled = false;
        _canUsePlayerStateMachine = false;
        _stateMachine = null;
        _isUsedAndEmpty = true;

        if (canRespawn)
            StartCoroutine(ActivateHealingZone());
    }

    private IEnumerator ActivateHealingZone()
    {
        yield return new WaitForSeconds(respawnTime);
        _isUsedAndEmpty = false;
        _healthPointsInZone = amountOfHealthPoints;
        _collider.enabled = true;
        _meshRenderer.enabled = true;
    }
}