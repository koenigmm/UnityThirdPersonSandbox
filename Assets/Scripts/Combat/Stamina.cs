using System;
using UnityEngine;

public class Stamina : MonoBehaviour
{
    public event Action OnStaminaChange;
    public float CurrentStamina { get; private set; }
    public bool CanRestore { get; set; } = true;
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float restoringInterval = 1f;
    [SerializeField] private float restoringAmountPerInterval = 5f;
    private float _timer;

    private void Start()
    {
        CurrentStamina = maxStamina;
        OnStaminaChange?.Invoke();
    }

    public void ReduceStamina(float staminaCost)
    {
        if (staminaCost < 0f)
        {
            Debug.LogError(
                $"You should use positive values staminaCost: {staminaCost} " +
                $"is handled as absolute value to prevent unwanted restoring");
        }

        staminaCost = Math.Abs(staminaCost);

        CurrentStamina = Mathf.Max(0, CurrentStamina - staminaCost);

        OnStaminaChange?.Invoke();
    }

    public float GetStaminaFraction() => CurrentStamina / maxStamina;

    private void Update()
    {
        if (!CanRestore) return;
        
        var isRestoring = !Mathf.Approximately(CurrentStamina, maxStamina);

        if (isRestoring && _timer > restoringInterval)
        {
            CurrentStamina = MathF.Min(maxStamina, CurrentStamina + restoringAmountPerInterval);
            OnStaminaChange?.Invoke();
        }

        if (!isRestoring || _timer > restoringInterval)
        {
            _timer = 0f;
        }

        _timer += Time.deltaTime;
    }
}