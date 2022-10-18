using System.Collections;
using UnityEngine;

public class DeactivateHintImage : MonoBehaviour
{
    [SerializeField] private Canvas hintCanvas;
    [SerializeField] private float delay;
    private Collider _collider;

    private void Awake() => _collider = GetComponent<Collider>();


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _collider.enabled = false;
        StartCoroutine(DeactivateCanvas());
    }

    private IEnumerator DeactivateCanvas()
    {
        yield return new WaitForSeconds(delay);
        hintCanvas.enabled = false;
    }
}
