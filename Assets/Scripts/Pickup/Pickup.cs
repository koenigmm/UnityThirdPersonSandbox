using System;
using SavingSystem;
using UnityEngine;

[RequireComponent(typeof(MovePickup), typeof(Collider))]
public class Pickup : SaveableEntity
{

    private bool _isUsed;
    private MovePickup _movePickup;
    private MeshRenderer[] _meshRenderer;
    private Collider _collider;
    [SerializeField] private PickUpType pickUpType;
    [SerializeField] private int amountOfItemsInPickup = 1;
    [SerializeField] private AmmoType ammoType;

    private void Awake()
    {
        _movePickup = GetComponent<MovePickup>();
        _meshRenderer = GetComponentsInChildren<MeshRenderer>();
        _collider = GetComponent<Collider>();
    }

    private void Start()
    {
        if (_isUsed)
        {
            DisablePickup();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (pickUpType)
        {
            case PickUpType.LevelUp:
                if (!other.TryGetComponent<LevelUpTokenInventory>(out var levelUpTokenInventory)) return;
                levelUpTokenInventory.IncreaseLevelUpTokens();
                DisablePickup();
                break;
            case PickUpType.HealingPotion:
                if (!other.TryGetComponent<HealingPotionsInventory>(out var healingPotionInventory)) return;
                healingPotionInventory.IncreaseAmountOfPotions(amountOfItemsInPickup);
                DisablePickup();
                break;
            case PickUpType.Ammunition:
                if (!other.TryGetComponent<AmmoInventory>(out var ammoInventory)) return;
                ammoInventory.IncreaseAmmo(ammoType, amountOfItemsInPickup);
                DisablePickup();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

#if UNITY_EDITOR
    private void Update()
    {
        SetGuid();
    }
#endif

    private void DisablePickup()
    {
        _isUsed = true;
        _collider.enabled = false;
        _movePickup.enabled = false;

        foreach (var component in _meshRenderer)
        {
            component.enabled = false;
        }
    }


    public override void PopulateSaveData(SaveData saveData)
    {
        var pickupData = new BoolWithID
        {
            uuid = uuid,
            savedBool = _isUsed
        };

        saveData.pickUpComponents.Add(pickupData);
    }

    public override void LoadFromSaveData(SaveData saveData)
    {
        foreach (var pickUp in saveData.pickUpComponents)
        {
            if (pickUp.uuid != uuid) continue;
            _isUsed = pickUp.savedBool;
        }
    }
}