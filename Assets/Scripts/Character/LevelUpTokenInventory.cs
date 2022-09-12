using System;
using SavingSystem;
using UnityEngine;

public class LevelUpTokenInventory : SaveableEntity
{
    public event Action OnTokenValueChange;
    public int AmountOfLevelUpTokens { get; private set; }
    private Level _level;


    private void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        _level = player.GetComponent<Level>();
    }


    public void IncreaseLevelUpTokens()
    {
        AmountOfLevelUpTokens++;
        OnTokenValueChange?.Invoke();
    }

    public void IncreaseHealthLevel()
    {
        if (AmountOfLevelUpTokens <= 0) return;
        _level.IncreaseHealthLevel();
        DecreaseLevelUpTokens();
    }

    public void IncreaseStaminaLevel()
    {
        if (AmountOfLevelUpTokens <= 0) return;
        _level.IncreaseStaminaLevel();
        DecreaseLevelUpTokens();
    }


    private void DecreaseLevelUpTokens()
    {
        AmountOfLevelUpTokens--;
        OnTokenValueChange?.Invoke();
    }

    public override void PopulateSaveData(SaveData saveData) => saveData.levelUpTokensInInventory = AmountOfLevelUpTokens;

    public override void LoadFromSaveData(SaveData saveData) => AmountOfLevelUpTokens = saveData.levelUpTokensInInventory;
}