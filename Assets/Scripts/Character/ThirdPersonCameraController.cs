using System;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    [NonSerialized] public bool canAim;

    [Header("Camera Settings")] 
    [SerializeField] private Transform cameraTarget;

    [SerializeField] private float minPitchAngle, maxPitchAngle;
    [SerializeField] private bool invertInputPitchValue = true;

    [Header("Camera Speed")] 
    [SerializeField] private float freeLookCameraSpeed = 5f;
    [SerializeField] private float aimCameraSpeed = 1f;

    private float _cameraSpeed;

    private InputReader _inputReader;
    private float _cameraTargetPitch, _cameraTargetYaw;

    private void Awake() => _inputReader = FindObjectOfType<InputReader>();

    private void Start() => _cameraTargetYaw = cameraTarget.transform.rotation.eulerAngles.y;

    private void Update()
    {
        SetCameraSpeed();
        RotateCameraTargetAndCharacterController();
    }

    private void SetCameraSpeed() => _cameraSpeed = _inputReader.IsAiming ? aimCameraSpeed : freeLookCameraSpeed;

    private void RotateCameraTargetAndCharacterController()
    {
        _cameraTargetYaw += _inputReader.LookPosition.x * Time.deltaTime * _cameraSpeed;

        if (invertInputPitchValue)
            _cameraTargetPitch -= _inputReader.LookPosition.y * Time.deltaTime * _cameraSpeed;
        else
            _cameraTargetPitch += _inputReader.LookPosition.y * Time.deltaTime * _cameraSpeed;

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