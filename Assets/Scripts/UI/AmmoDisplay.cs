using TMPro;
using UnityEngine;

public class AmmoDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoDisplayTextField;
    private RangedWeapon _currentRangedWeapon;
    private AmmunitionInventory _ammunitionInventory;
    private PlayerStateMachine _playerStateMachine;

    private void Awake()
    {
        var player = GameObject.FindWithTag("Player");
        _playerStateMachine = player.GetComponent<PlayerStateMachine>();
        _currentRangedWeapon = _playerStateMachine.CurrentRangedWeapon;
        _ammunitionInventory = player.GetComponent<AmmunitionInventory>();
    }
    

    private void OnEnable()
    {
        _currentRangedWeapon.OnAmmoChange += UpdateTextField;
        _ammunitionInventory.OnAmmoChange += UpdateTextField;
    }

    private void OnDisable()
    {
        _currentRangedWeapon.OnAmmoChange -= UpdateTextField;
        _ammunitionInventory.OnAmmoChange -= UpdateTextField;
    }

    private void UpdateTextField()
    {
        var ammoDisplayText = $"{_currentRangedWeapon.CurrentAmmunition} / {_ammunitionInventory.GetAmountOfAmmo(_currentRangedWeapon.AmmunitionType)}";
        ammoDisplayTextField.text = ammoDisplayText;
    }
}
