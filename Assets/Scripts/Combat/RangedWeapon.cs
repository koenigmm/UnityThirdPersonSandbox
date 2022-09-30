using System;
using System.Collections;
using SavingSystem;
using UnityEngine;
using UnityEngine.VFX;

public class RangedWeapon : SaveableEntity
{
    // public event Action OnAmmunitionChange;

    [Header("Weapon Attributes")] [SerializeField]
    private float damage = 10f;

    [SerializeField] private float range = 100f;
    [SerializeField] private float reloadingTime = 1.5f;
    [Header("Ammo")] [SerializeField] private int maxAmmoInWeapon = 10;
    [SerializeField] private AmmunitionInventory ammunitionInventory;
    [SerializeField] private AmmunitionType ammunitionType;
    [SerializeField] private VisualEffect muzzleVFX;

    public int CurrentAmmunition { get; private set; }

    public AmmunitionType AmmunitionType => ammunitionType;
    public float ReloadingTime => reloadingTime;

    private void Awake()
    {
        if (ammunitionInventory != null) return;
        ammunitionInventory = GetComponentInParent<AmmunitionInventory>();
    }

    private void OnEnable()
    {
        muzzleVFX.enabled = true;
        muzzleVFX.Stop();
    }
    
#if UNITY_EDITOR
    private void Update()
    {
        SetGuid();
    }
#endif


    public void Shoot()
    {
        if (CurrentAmmunition <= 0) return;

        var screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        var rayToCenter = Camera.main.ScreenPointToRay(screenCenter);

        var hasHit = Physics.Raycast(rayToCenter, out var raycastHit, range);
        if (!hasHit) return;

        CurrentAmmunition = Math.Max(0, CurrentAmmunition - 1);
        muzzleVFX.Play();

        if (raycastHit.transform.TryGetComponent(out Health enemyHealth))
            enemyHealth.DealDamage(damage);
    }

    public bool TryReload()
    {
        var currentAmmoBeforeReload = CurrentAmmunition;
        if (CurrentAmmunition == maxAmmoInWeapon) return false;
        var ammoNeededToFillWeapon = maxAmmoInWeapon - CurrentAmmunition;
        CurrentAmmunition += ammunitionInventory.GetAmmo(ammunitionType, ammoNeededToFillWeapon);
        return CurrentAmmunition != currentAmmoBeforeReload;
    }

    public override void PopulateSaveData(SaveData saveData)
    {
        var weaponData = new IntWithID
        {
            uuid = uuid,
            savedInt = CurrentAmmunition
        };

        saveData.ammunitionInWeapons.Add(weaponData);
    }

    public override void LoadFromSaveData(SaveData saveData)
    {
        foreach (var weapon in saveData.ammunitionInWeapons)
        {
            if (weapon.uuid != uuid) continue;
            CurrentAmmunition = weapon.savedInt;
        }
    }
}