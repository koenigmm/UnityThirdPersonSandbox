using System;
using System.Collections.Generic;
using SavingSystem;
using UnityEngine;


public class AmmunitionInventory : SaveableEntity
{
    public event Action OnAmmoChange;
    
    [SerializeField] private AmmoInventoryStartValue[] ammoInventoryStartValues;
    private Dictionary<AmmunitionType, int> _ammunition = new();
    private bool isRestored;

    [Serializable]
    private class AmmoInventoryStartValue
    {
        public AmmunitionType type;
        public int amount;
    }

    private void Start()
    {
        if (isRestored) return;
        
        // Get first values for ammo dictionary from SerializeField
        foreach (var ammo in ammoInventoryStartValues)
        {
            _ammunition[ammo.type] = ammo.amount;
        }
        
        OnAmmoChange?.Invoke();
    }

    public void IncreaseAmmo(AmmunitionType ammunitionType, int amount = 1)
    {
        _ammunition[ammunitionType] += Math.Abs(amount);
        OnAmmoChange?.Invoke();
    }

    public int GetAmmo(AmmunitionType ammunitionType, int amount)
    {
        OnAmmoChange?.Invoke();
        var ammoToReturn =  Math.Min(_ammunition[ammunitionType], amount);
        _ammunition[ammunitionType] -= ammoToReturn;
        return ammoToReturn;
    }

    public int GetAmountOfAmmo(AmmunitionType ammunitionType) => _ammunition[ammunitionType];
    
    public override void PopulateSaveData(SaveData saveData)
    {
        var ammunitionData = new AmmunitionInventoryData
        {
            bullets = _ammunition[AmmunitionType.Bullet],
            heavyBullets = _ammunition[AmmunitionType.HeavyBullet]
        };

        saveData.ammunitionInventoryData = ammunitionData;
    }

    public override void LoadFromSaveData(SaveData saveData)
    {
        isRestored = true;
        _ammunition[AmmunitionType.HeavyBullet] = saveData.ammunitionInventoryData.heavyBullets;
        _ammunition[AmmunitionType.Bullet] = saveData.ammunitionInventoryData.bullets;
        OnAmmoChange?.Invoke();
    }
}