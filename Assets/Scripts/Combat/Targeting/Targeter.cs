using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    private List<Target> _targets = new List<Target>();
    [SerializeField] private CinemachineTargetGroup cineTargetGroup;
    public Target CurrentTarget { get; private set; }

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

        CurrentTarget = _targets[0];
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