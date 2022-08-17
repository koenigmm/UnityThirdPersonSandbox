using UnityEngine;

public class Pickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<LevelUpToken>(out var inventory)) return;
        inventory.IncreaseLevelUpTokens();
        Destroy(gameObject);
    }
}