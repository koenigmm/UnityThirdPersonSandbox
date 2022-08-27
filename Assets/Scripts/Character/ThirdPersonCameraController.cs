using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    public bool canAim;
    
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private float minPitchAngle, maxPitchAngle;
    [SerializeField] private float cameraSpeed = 5f;
    [SerializeField] private bool invertInputPitchValue = true;
    
    private InputReader _inputReader;
    private float _cameraTargetPitch, _cameraTargetYaw;

    private void Awake() => _inputReader = FindObjectOfType<InputReader>();

    private void Start() => _cameraTargetYaw = cameraTarget.transform.rotation.eulerAngles.y;

    private void Update() => RotateCameraTargetAndCharacterController();

    private void RotateCameraTargetAndCharacterController()
    {
        _cameraTargetYaw += _inputReader.LookPosition.x * Time.deltaTime * cameraSpeed;

        if (invertInputPitchValue)
            _cameraTargetPitch -= _inputReader.LookPosition.y * Time.deltaTime * cameraSpeed;
        else
            _cameraTargetPitch += _inputReader.LookPosition.y * Time.deltaTime * cameraSpeed;

        _cameraTargetYaw = ClampAngle(_cameraTargetYaw, float.MinValue, float.MaxValue);
        _cameraTargetPitch = ClampAngle(_cameraTargetPitch, minPitchAngle, maxPitchAngle);

        cameraTarget.rotation = Quaternion.Euler(_cameraTargetPitch, _cameraTargetYaw, 0f);

        if (!_inputReader.IsAiming || !canAim) return;
        LookAtAimTarget();
        
    }

    public void LookAtAimTarget(float interpolationRatio = 20f)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, _cameraTargetYaw, 0f),
            Time.deltaTime * interpolationRatio);
    }

    private static float ClampAngle(float angle, float minAngle, float maxAngle)
    {
        if (angle < -360f) angle += 360f;
        if (angle > 360f) angle -= 360f;

        return Mathf.Clamp(angle, minAngle, maxAngle);
    }
}