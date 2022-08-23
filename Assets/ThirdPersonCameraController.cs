using System;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private float minPitchAngle, maxPitchAngle;
    [SerializeField] private float cameraSpeed = 5f;
    [SerializeField] private bool invertInputPitchValue = true;

    private InputReader _inputReader;
    private float _cameraTargetPitch, _cameraTargetYaw;

    private void Awake()
    {
        _inputReader = FindObjectOfType<InputReader>();
    }

    private void Start()
    {
        _cameraTargetYaw = cameraTarget.transform.rotation.eulerAngles.y;
    }


    private void LateUpdate()
    {
        _cameraTargetYaw += _inputReader.LookPosition.x * Time.deltaTime * cameraSpeed;


        if (invertInputPitchValue)
            _cameraTargetPitch -= _inputReader.LookPosition.y * Time.deltaTime * cameraSpeed;
        else
            _cameraTargetPitch += _inputReader.LookPosition.y * Time.deltaTime * cameraSpeed;

        _cameraTargetYaw = ClampAngle(_cameraTargetYaw, float.MinValue, float.MaxValue);
        _cameraTargetPitch = ClampAngle(_cameraTargetPitch, minPitchAngle, maxPitchAngle);


        cameraTarget.transform.rotation = Quaternion.Euler(_cameraTargetPitch, _cameraTargetYaw, 0f);
    }

    private static float ClampAngle(float angle, float minAngle, float maxAngle)
    {
        if (angle < -360f) angle += 360f;
        if (angle > 360f) angle -= 360f;

        return Mathf.Clamp(angle, minAngle, maxAngle);
    }
}