using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalLightFader : MonoBehaviour
{
    [SerializeField] private Light directionalLight;
    [SerializeField] private float targetValue;
    [SerializeField] private float fadingDuration = 0.75f;
    [SerializeField] private float lerpInterpolationValue = 1.5f;
    [SerializeField] private string playerTag = "Player";
    private bool _isFading;
    private float _timer;

    private void Update()
    {
        if (!_isFading) return;

        _timer += Time.deltaTime;
        directionalLight.intensity = Mathf.Lerp(directionalLight.intensity, targetValue, Time.deltaTime * lerpInterpolationValue);

        if (_timer >= fadingDuration)
        {
            _timer = 0f;
            _isFading = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        _isFading = true;
    }
}
