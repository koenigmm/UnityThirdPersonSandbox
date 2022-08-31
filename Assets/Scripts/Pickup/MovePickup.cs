using UnityEngine;

public class MovePickup : MonoBehaviour
{
    [SerializeField] private Vector3 anglePerSecond;
    [SerializeField] private bool shouldMoveUpAndDown = true;
    [SerializeField] private Vector3 maxPosition;
    [SerializeField] private float moveInterpolationSpeed = 0.5f;
    private Vector3 _defaultPosition;
    private Vector3 _targetPosition;

    private void Start() => _defaultPosition = transform.position;

    private void Update()
    {
        RotatePickup();
        
        if (shouldMoveUpAndDown) 
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

        if (Vector3.Distance(transform.position, maxPosition) <= distanceTolerance)
        {
            _targetPosition = _defaultPosition;
        }

        if (Vector3.Distance(transform.position, _defaultPosition) <= distanceTolerance)
        {
            _targetPosition = maxPosition;
        }

        transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * moveInterpolationSpeed);
    }
    
}