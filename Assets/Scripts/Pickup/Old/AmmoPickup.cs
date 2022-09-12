using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    [SerializeField] private AmmunitionType ammunitionType;
    [SerializeField] private int amountOfAmmo;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out AmmunitionInventory ammoInventory))
        {
            ammoInventory.IncreaseAmmo(ammunitionType, amountOfAmmo);
            gameObject.SetActive(false);
        }
    }
}
