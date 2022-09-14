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
        // TODO ranged weapon change / weapon switcher
        _currentRangedWeapon.OnAmmunitionChange += UpdateTextField;
        _ammunitionInventory.OnAmmunitionChange += UpdateTextField;
    }

    private void OnDisable()
    {
        // TODO ranged weapon change / weapon switcher
        _currentRangedWeapon.OnAmmunitionChange -= UpdateTextField;
        _ammunitionInventory.OnAmmunitionChange -= UpdateTextField;
    }

    private void UpdateTextField()
    {
        var ammoDisplayText = $"{_currentRangedWeapon.CurrentAmmunition} / {_ammunitionInventory.GetAmountOfAmmo(_currentRangedWeapon.AmmunitionType)}";
        ammoDisplayTextField.text = ammoDisplayText;
    }
}
