using System;
using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public event Action OnAmmoChange;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float range = 100f;
    [SerializeField] private int maxAmmoInWeapon = 10;
    [SerializeField] private int currentAmmo = 0;
    [SerializeField] private AmmoInventory ammoInventory;
    [SerializeField] private AmmoType ammoType;

    public int CurrentAmmo => currentAmmo;
    public AmmoType AmmoType => ammoType;
    

    public void Shoot()
    {
        if (currentAmmo <= 0) return;

        var screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        var rayToCenter = Camera.main.ScreenPointToRay(screenCenter);

        var hasHit = Physics.Raycast(rayToCenter, out var raycastHit, range);
        if (!hasHit) return;

        currentAmmo = Math.Max(0, currentAmmo - 1);
        OnAmmoChange?.Invoke();

        if (raycastHit.transform.TryGetComponent(out Health enemyHealth))
            enemyHealth.DealDamage(damage);
    }

    public bool TryReload()
    {
        var currentAmmoBeforeReload = currentAmmo; 
        if (currentAmmo == maxAmmoInWeapon) return false;
        var ammoNeededToFillWeapon = maxAmmoInWeapon - currentAmmo;
        currentAmmo += ammoInventory.GetAmmo(ammoType, ammoNeededToFillWeapon);
        OnAmmoChange?.Invoke();
        return currentAmmo != currentAmmoBeforeReload;
    }
}