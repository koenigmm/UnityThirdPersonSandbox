using System;
using UnityEngine;

public class HealingPotionsInventory : MonoBehaviour
{
    [Tooltip("Default is MaxHealth")]
    [SerializeField] private float healingPotionHealthPoints;

    public event Action OnPotionUpdate;
    public int AmountOfPotions { get; private set; }
    private Health _playerHealth;
    private InputReader _inputReader;
    
    private void Awake()
    {
        var player = GameObject.FindWithTag("Player");
        _playerHealth = player.GetComponent<Health>();
        _inputReader = player.GetComponent<InputReader>();
    }

    private void OnEnable() => _inputReader.ConsumePotionEvent += UseHealingPotion;

    private void OnDisable() => _inputReader.ConsumePotionEvent -= UseHealingPotion;
    
    public void IncreaseAmountOfPotions(int amountOfPotions = 1)
    {
        amountOfPotions = Math.Abs(amountOfPotions);
        AmountOfPotions += amountOfPotions;
        OnPotionUpdate?.Invoke();
    }

    private bool DecreaseAmountOfHealthPotions()
    {
        if (AmountOfPotions - 1 < 0) return false;
        AmountOfPotions--;
        OnPotionUpdate?.Invoke();
        return true;
    }

    public void UseHealingPotion()
    {
        if (DecreaseAmountOfHealthPotions()) _playerHealth.Heal(_playerHealth.MaxHealth);
    }
    
    
}