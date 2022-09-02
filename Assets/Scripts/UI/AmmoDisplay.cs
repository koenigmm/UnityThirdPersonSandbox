using TMPro;
using UnityEngine;

public class AmmoDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoDisplayTextField;
    private RangedWeapon _currentRangedWeapon;
    private AmmoInventory _ammoInventory;
    private PlayerStateMachine _playerStateMachine;

    private void Awake()
    {
        var player = GameObject.FindWithTag("Player");
        _playerStateMachine = player.GetComponent<PlayerStateMachine>();
        _currentRangedWeapon = _playerStateMachine.CurrentRangedWeapon;
        _ammoInventory = player.GetComponent<AmmoInventory>();
    }
    

    private void OnEnable()
    {
        _currentRangedWeapon.OnAmmoChange += UpdateTextField;
        _ammoInventory.OnAmmoChange += UpdateTextField;
    }

    private void OnDisable()
    {
        _currentRangedWeapon.OnAmmoChange -= UpdateTextField;
        _ammoInventory.OnAmmoChange -= UpdateTextField;
    }

    private void UpdateTextField()
    {
        var ammoDisplayText = $"{_currentRangedWeapon.CurrentAmmo} / {_ammoInventory.GetAmountOfAmmo(_currentRangedWeapon.AmmoType)}";
        ammoDisplayTextField.text = ammoDisplayText;
    }
}
