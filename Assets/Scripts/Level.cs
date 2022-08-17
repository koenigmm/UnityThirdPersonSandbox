using System;
using UnityEngine;

public class Level : MonoBehaviour
{
    public event Action OnHealthLevelUp;
    public event Action OnStaminaLevelUp;
    [SerializeField] private Progression[] progressions;
    private int _healthLevel;
    private int _staminaLevel;

    private int _healthLevelCount;
    private int _staminaLevelCount;

    private void Start()
    {
        _healthLevelCount = GetLevelCount(PlayerAttributes.Health);
        _staminaLevelCount = GetLevelCount(PlayerAttributes.Stamina);
    }


    private void IncreaseLevel(PlayerAttributes playerAttribute)
    {
        if (playerAttribute == PlayerAttributes.Health)
        {
            if (_healthLevel + 1 > _healthLevelCount -1) return;
            _healthLevel++;
            OnHealthLevelUp?.Invoke();
        }

        if (playerAttribute == PlayerAttributes.Stamina)
        {
            if (_staminaLevel + 1 > _staminaLevelCount -1) return;
            _staminaLevel++;
            OnStaminaLevelUp?.Invoke();
        }
    }

    public void IncreaseHealthLevel() => IncreaseLevel(PlayerAttributes.Health);
    public void IncreaseStaminaLevel() => IncreaseLevel(PlayerAttributes.Stamina);


    public float GetMaxHealth() => GetMaxValueFromProgression(PlayerAttributes.Health, _healthLevel);

    public float GetMaxStamina() => GetMaxValueFromProgression(PlayerAttributes.Stamina, _staminaLevel);


    private float GetMaxValueFromProgression(PlayerAttributes playerAttribute, int level)
    {
        var maxValue = 0f;
        foreach (var progression in progressions)
        {
            if (progression.playerAttribute == playerAttribute)
                maxValue = progression.levels[level];
        }

        return maxValue;
    }

    private int GetLevelCount(PlayerAttributes playerAttribute)
    {
        var count = 0;
        foreach (var progression in progressions)
        {
            if (progression.playerAttribute == playerAttribute)
                count = progression.levels.Length;
        }

        return count;
    }

    [Serializable]
    private class Progression
    {
        public PlayerAttributes playerAttribute;
        public float[] levels;
    }
}