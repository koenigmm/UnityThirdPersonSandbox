using UnityEngine;
using System.Collections;

public class DirectionalLightFader : MonoBehaviour
{
    [SerializeField] private float fadingDuration = 1f;
    [SerializeField] LightWithData[] lightWithData;
    [SerializeField] private string playerTag = "Player";
    private bool _isFading;
    private float _timer;
    private float[] fractionsPerSecond;

    private void Update()
    {
        if (!_isFading) return;

        _timer += Time.deltaTime;

        foreach (var light in lightWithData)
        {
            light.DirectionalLight.intensity = Mathf.MoveTowards(light.DirectionalLight.intensity, light.TargatValue, Time.deltaTime * light.IntensityChangeRate);
        }

        if (_timer < fadingDuration) return;

        _timer = 0f;
        _isFading = false;

    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        _isFading = true;
    }

    private void OnDrawGizmos()
    {
        const float radius = 1f;
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, radius);
    }

    [System.Serializable]
    private struct LightWithData
    {
        public Light DirectionalLight;
        public float TargatValue;
        public float IntensityChangeRate;
    }
}
