using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class RangedWeapon : MonoBehaviour
{
    public event Action OnAmmoChange;
    [Header("Weapon Attributes")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float range = 100f;
    [SerializeField] private float reloadingTime = 1.5f;
    [Header("Ammo")]
    [SerializeField] private int maxAmmoInWeapon = 10;
    [SerializeField] private int currentAmmo = 0;
    [SerializeField] private AmmoInventory ammoInventory;
    [SerializeField] private AmmoType ammoType;
    [SerializeField] private VisualEffect muzzleVFX;

    public int CurrentAmmo => currentAmmo;
    public AmmoType AmmoType => ammoType;
    public float ReloadingTime => reloadingTime;

    private void Awake()
    {
        if (ammoInventory != null) return;
        ammoInventory = GetComponentInParent<AmmoInventory>();
    }

    private void Start()
    {
        muzzleVFX.enabled = true;
        muzzleVFX.Stop();
    }


    public void Shoot()
    {
        if (currentAmmo <= 0) return;

        var screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        var rayToCenter = Camera.main.ScreenPointToRay(screenCenter);

        var hasHit = Physics.Raycast(rayToCenter, out var raycastHit, range);
        if (!hasHit) return;

        currentAmmo = Math.Max(0, currentAmmo - 1);
        OnAmmoChange?.Invoke();
        muzzleVFX.Play();

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