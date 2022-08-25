using TMPro;
using UnityEngine;

public class AmmoDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoDisplayTextField;
    private Weapon _currentWeapon;
    private AmmoInventory _ammoInventory;
    private PlayerStateMachine _playerStateMachine;

    private void Awake()
    {
        var player = GameObject.FindWithTag("Player");
        _playerStateMachine = player.GetComponent<PlayerStateMachine>();
        _currentWeapon = _playerStateMachine.CurrentWeapon;
        _ammoInventory = player.GetComponent<AmmoInventory>();
    }
    

    private void OnEnable()
    {
        _currentWeapon.OnAmmoChange += UpdateTextField;
        _ammoInventory.OnAmmoChange += UpdateTextField;
    }

    private void OnDisable()
    {
        _currentWeapon.OnAmmoChange -= UpdateTextField;
        _ammoInventory.OnAmmoChange -= UpdateTextField;
    }

    private void UpdateTextField()
    {
        var ammoDisplayText = $"{_currentWeapon.CurrentAmmo} / {_ammoInventory.GetAmountOfAmmo(_currentWeapon.AmmoType)}";
        ammoDisplayTextField.text = ammoDisplayText;
    }
}
