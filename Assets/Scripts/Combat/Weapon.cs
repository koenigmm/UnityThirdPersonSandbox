using System;
using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float range = 100f;
    [SerializeField] private int maxAmmoInWeapon = 10;
    [SerializeField] private int currentAmmo = 0;
    [SerializeField] private AmmoInventory ammoInventory;
    [SerializeField] private AmmoType ammoType;

    private InputReader _inputReader;

    private void Awake() => _inputReader = FindObjectOfType<InputReader>();

    private void OnEnable() => _inputReader.OnReloadWeapon += Reload;

    private void OnDisable() => _inputReader.OnReloadWeapon -= Reload;


    public void Shoot()
    {
        if (currentAmmo <= 0) return;

        var screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        var rayToCenter = Camera.main.ScreenPointToRay(screenCenter);

        var hasHit = Physics.Raycast(rayToCenter, out var raycastHit, range);
        if (!hasHit) return;

        currentAmmo = Math.Max(0, currentAmmo - 1);

        if (raycastHit.transform.TryGetComponent(out Health enemyHealth))
            enemyHealth.DealDamage(damage);
    }

    private void Reload()
    {
        if (currentAmmo == maxAmmoInWeapon) return;
        var ammoNeededToFillWeapon = maxAmmoInWeapon - currentAmmo;
        currentAmmo += ammoInventory.GetAmmo(ammoType, ammoNeededToFillWeapon);
    }
}