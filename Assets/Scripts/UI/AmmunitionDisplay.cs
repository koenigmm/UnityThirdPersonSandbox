using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class AmmunitionDisplay : MonoBehaviour
{
    [FormerlySerializedAs("ammoDisplayTextField")] [SerializeField] private TextMeshProUGUI ammunitionDisplayTextField;
    
    private AmmunitionInventory _ammunitionInventory;
    private PlayerStateMachine _playerStateMachine;

    private void Awake()
    {
        var player = GameObject.FindWithTag("Player");
        _playerStateMachine = player.GetComponent<PlayerStateMachine>();
        _ammunitionInventory = player.GetComponent<AmmunitionInventory>();
    }

    private void OnEnable()
    {
        // TODO ranged weapon change / weapon switcher
        _ammunitionInventory.OnAmmunitionChange += UpdateTextField;
        _playerStateMachine.InputReader.OnShooting += HandleShooting;
        _playerStateMachine.InputReader.OnReloadWeapon += HandleReload;
        StartCoroutine(UpdateTextFieldWithDelay());
    }


    private void OnDisable()
    {
        // TODO ranged weapon change / weapon switcher
        _ammunitionInventory.OnAmmunitionChange -= UpdateTextField;
        _playerStateMachine.InputReader.OnShooting -= HandleShooting;
        _playerStateMachine.InputReader.OnReloadWeapon -= HandleReload;
    }

    private void HandleReload() => StartCoroutine(UpdateTextFieldWithDelay(_playerStateMachine.CurrentRangedWeapon.ReloadingTime));

    private void HandleShooting() => StartCoroutine(UpdateTextFieldWithDelay());

    private IEnumerator UpdateTextFieldWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        UpdateTextField();
    }

    private IEnumerator UpdateTextFieldWithDelay()
    {
        yield return new WaitForEndOfFrame();
        UpdateTextField();
    }
    
    private void UpdateTextField()
    {
        var ammoDisplayText =
            $"{_playerStateMachine.CurrentRangedWeapon.CurrentAmmunition} /" +
            $" {_ammunitionInventory.GetAmountOfAmmo(_playerStateMachine.CurrentRangedWeapon.AmmunitionType)}";
        ammunitionDisplayTextField.text = ammoDisplayText;
    }
}