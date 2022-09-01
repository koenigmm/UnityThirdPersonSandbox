using UnityEngine;

public class MovePickup : MonoBehaviour
{
    [Header("Behaviour")]
    [SerializeField] private bool canMoveBetweenTwoPoints = true;
    [SerializeField] private bool canRotate = true;
    [Header("Values")]
    [SerializeField] private Vector3 anglePerSecond;
    [Tooltip(" The target position equals the current position plus the offset")]
    [SerializeField] private Vector3 offset;
    [SerializeField] private float moveInterpolationSpeed = 0.5f;
    private Vector3 _defaultPosition;
    private Vector3 _currentTarget;

    private void Start() => _defaultPosition = transform.position;

    private void Update()
    {
        if (canRotate)
            RotatePickup();

        if (canMoveBetweenTwoPoints)
            MoveUpAndDown();
    }

    private void RotatePickup()
    {
        var frameRateIndependentAnglePerSecond = anglePerSecond * Time.deltaTime;
        transform.Rotate(frameRateIndependentAnglePerSecond, Space.World);
    }

    private void MoveUpAndDown()
    {
        const float distanceTolerance = 0.05f;

        if (Vector3.Distance(transform.position, _defaultPosition + offset) <= distanceTolerance)
        {
            _currentTarget = _defaultPosition;
        }

        if (Vector3.Distance(transform.position, _defaultPosition) <= distanceTolerance)
        {
            _currentTarget = _defaultPosition + offset;
        }

        transform.position = Vector3.Lerp(transform.position, _currentTarget, Time.deltaTime * moveInterpolationSpeed);
    }
}