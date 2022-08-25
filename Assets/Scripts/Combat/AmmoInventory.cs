using System;
using System.Collections.Generic;
using UnityEngine;


public class AmmoInventory : MonoBehaviour
{
    public event Action OnAmmoChange;
    
    [SerializeField] private AmmoInventoryStartValue[] ammoInventoryStartValues;
    private Dictionary<AmmoType, int> _ammo = new();

    [Serializable]
    private class AmmoInventoryStartValue
    {
        public AmmoType type;
        public int amount;
    }

    private void Start()
    {
        // Get first values for ammo dictionary from SerializeField
        foreach (var ammo in ammoInventoryStartValues)
        {
            _ammo[ammo.type] = ammo.amount;
        }
        
        OnAmmoChange?.Invoke();
    }

    public void IncreaseAmmo(AmmoType ammoType, int amount = 1)
    {
        _ammo[ammoType] += Math.Abs(amount);
        OnAmmoChange?.Invoke();
    }

    public int GetAmmo(AmmoType ammoType, int amount)
    {
        OnAmmoChange?.Invoke();
        var ammoToReturn =  Math.Min(_ammo[ammoType], amount);
        _ammo[ammoType] -= ammoToReturn;
        return ammoToReturn;
    }

    public int GetAmountOfAmmo(AmmoType ammoType) => _ammo[ammoType];
}