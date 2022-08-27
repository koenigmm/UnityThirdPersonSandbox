using System;
using UnityEngine;
using UnityEngine.Serialization;

public class HealingPotionsInventory : MonoBehaviour
{
    [Tooltip("Default is MaxHealth"), Range(1, 100)]
    [SerializeField] private int restoringPercentage = 100;

    private float _restoringFraction;

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

    private void Start() => _restoringFraction = restoringPercentage / 100f;


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

    private void UseHealingPotion()
    {
        if (DecreaseAmountOfHealthPotions()) 
            _playerHealth.Heal( _playerHealth.MaxHealth * _restoringFraction);
    }
    
    
}