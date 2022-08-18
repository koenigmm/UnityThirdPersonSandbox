using UnityEngine;

public class HealingPotionPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<HealingPotionsInventory>(out var inventory)) return;
        inventory.IncreaseAmountOfPotions();
        Destroy(gameObject);
    }
}
