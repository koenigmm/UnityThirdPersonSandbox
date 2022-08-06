using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    private List<Target> _targets = new List<Target>();
    [SerializeField] private CinemachineTargetGroup cineTargetGroup;
    public Target CurrentTarget { get; private set; }

    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Target target)) return;
        _targets.Add(target);
        target.OnDestroyed += RemoveTarget;
    }

    private void RemoveTarget(Target target)
    {
        if (CurrentTarget == target)
        {
            cineTargetGroup.RemoveMember(CurrentTarget.transform);
            CurrentTarget = null;
        }

        target.OnDestroyed -= RemoveTarget;
        _targets.Remove(target);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out Target target)) return;
        RemoveTarget(target);
    }

    public bool SelectTarget()
    {
        if (_targets.Count == 0) return false;

        Target closestTarget = null;
        var closestTargetDistance = Mathf.Infinity;

        foreach (var target in _targets)
        {
            Vector2 viewPos = _mainCamera.WorldToViewportPoint(target.transform.position);

            if (viewPos.x > 1f || viewPos.x < 0f || viewPos.y > 1f || viewPos.y < 0f)
            {
                continue;
            }

            var toCenter = viewPos - new Vector2(0.5f, 0.5f);
            if (toCenter.sqrMagnitude < closestTargetDistance)
            {
                closestTarget = target;
                closestTargetDistance = toCenter.sqrMagnitude;
            }
        }

        if (closestTarget == null) return false;
        CurrentTarget = closestTarget;
        cineTargetGroup.AddMember(CurrentTarget.transform, 1f, 2f);

        return true;
    }

    public void Cancel()
    {
        if (CurrentTarget == null) return;
        cineTargetGroup.RemoveMember(CurrentTarget.transform);
        CurrentTarget = null;
    }
}