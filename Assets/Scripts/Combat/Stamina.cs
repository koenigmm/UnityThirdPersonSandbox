using System;
using UnityEngine;

public class Stamina : MonoBehaviour
{
    public event Action OnStaminaChange;
    public float MaxStamina { get; private set; }
    public float CurrentStamina { get; private set; }
    public bool CanRestore { get; set; } = true;
    [SerializeField] private float restoringInterval = 1f;
    [SerializeField] private float restoringAmountPerInterval = 5f;
    private float _timer;
    private Level _level;

    private void Awake()
    {
        _level = GetComponent<Level>();
    }

    private void Start()
    {
        MaxStamina = _level.GetMaxStamina();
        CurrentStamina = MaxStamina;
        OnStaminaChange?.Invoke();
    }

    private void OnEnable() => _level.OnStaminaLevelUp += HandleLevelUp;

    private void OnDisable() => _level.OnStaminaLevelUp -= HandleLevelUp;

    private void HandleLevelUp() => MaxStamina = _level.GetMaxStamina();

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

    public float GetStaminaFraction() => CurrentStamina / MaxStamina;

    private void Update()
    {
        if (!CanRestore) return;
        
        var isRestoring = !Mathf.Approximately(CurrentStamina, MaxStamina);
        
        if (isRestoring && _timer > restoringInterval)
        {
            CurrentStamina = MathF.Min(MaxStamina, CurrentStamina + restoringAmountPerInterval);
            OnStaminaChange?.Invoke();
        }

        if (!isRestoring || _timer > restoringInterval)
        {
            _timer = 0f;
        }

        _timer += Time.deltaTime;
    }
}