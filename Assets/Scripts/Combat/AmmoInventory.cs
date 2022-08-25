using System;
using System.Collections.Generic;
using UnityEngine;


public class AmmoInventory : MonoBehaviour
{
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
    }

    public void IncreaseAmmo(AmmoType ammoType, int amount = 1)
    {
        _ammo[ammoType] += Math.Abs(amount);
    }

    public int GetAmmo(AmmoType ammoType, int amount)
    {
        var ammoToReturn =  Math.Min(_ammo[ammoType], amount);
        _ammo[ammoType] -= ammoToReturn;
        print(_ammo[ammoType]);
        return ammoToReturn;
    }
    
}