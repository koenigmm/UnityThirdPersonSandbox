using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    [SerializeField] private AmmoType ammoType;
    [SerializeField] private int amountOfAmmo;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out AmmoInventory ammoInventory))
        {
            ammoInventory.IncreaseAmmo(ammoType, amountOfAmmo);
            gameObject.SetActive(false);
        }
    }
}
