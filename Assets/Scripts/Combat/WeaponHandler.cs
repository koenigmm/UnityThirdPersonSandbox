using System;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private Collider weaponCollider;
    
    public void EnableWeapon()
    {
        weaponCollider.enabled = true;
    }

    public void DisableWeapon ()
    {
        weaponCollider.enabled = false;
    }
}
