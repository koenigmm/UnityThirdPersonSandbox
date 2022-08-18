using UnityEngine;

public class LevelUpPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<LevelUpTokenInventory>(out var inventory)) return;
        inventory.IncreaseLevelUpTokens();
        Destroy(gameObject);
    }
}