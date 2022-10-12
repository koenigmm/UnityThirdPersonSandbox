using UnityEngine;

public class DirectionalLightFader : MonoBehaviour
{
    [SerializeField] private float fadingDuration = 1f;
    [SerializeField] LightWithData[] lightWithData;
    [SerializeField] private string playerTag = "Player";
    private bool _isFading;
    private float _timer;

    private void Update()
    {
        if (!_isFading) return;

        _timer += Time.deltaTime;

        foreach (var light in lightWithData)
        {
            light.DirectionalLight.intensity = Mathf.Lerp(light.DirectionalLight.intensity, light.TargatValue, Time.deltaTime * light.LerpInterpolationRatio);
        }

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
        public float LerpInterpolationRatio;
    }
}
