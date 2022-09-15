using UnityEngine;

public class Waypoint : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        const float waypointRadius = 0.1f;

        for (var i = 0; i < transform.childCount; i++)
        {
            var j = GetNextIndex(i);
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));
            Gizmos.DrawSphere(GetWaypoint(i), waypointRadius);
        }
    }

    public Vector3 GetWaypoint(int index) => transform.GetChild(index).position;

    public int GetNextIndex(int index)
    {
        if (index + 1 < transform.childCount) return index + 1;
        return 0;
    }
}