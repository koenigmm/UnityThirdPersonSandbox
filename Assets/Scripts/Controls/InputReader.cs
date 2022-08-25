using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
    public Vector2 MovementValue { get; private set; }
    public event Action JumpEvent;
    public event Action DodgeEvent;
    public event Action TargetEvent;
    public event Action InteractEvent;
    public event Action ShowAttributesEvent;
    public event Action ConsumePotionEvent;
    public event Action OnReloadWeapon;

    public bool IsAttacking { get; private set; }
    public bool IsBlocking { get; private set; }
    public bool IsAiming { get; private set; }
    public Vector2 LookPosition { get; private set; }

    private Controls _controls;

    private void Start()
    {
        _controls = new Controls();
        _controls.Player.SetCallbacks(this);
        _controls.Player.Enable();
    }

    private void OnDestroy()
    {
        _controls.Player.Disable();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        JumpEvent?.Invoke();
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        DodgeEvent?.Invoke();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MovementValue = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        LookPosition = context.ReadValue<Vector2>();
    }

    public void OnTarget(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        TargetEvent?.Invoke();
    }


    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
            IsAttacking = true;
        if (context.canceled)
            IsAttacking = false;
    }

    public void OnBlock(InputAction.CallbackContext context)
    {
        if (context.performed)
            IsBlocking = true;
        if (context.canceled)
            IsBlocking = false;
    }

    public void OnExit(InputAction.CallbackContext context)
    {
        Application.Quit();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        InteractEvent?.Invoke();
    }

    public void OnToggleAttributes(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        ShowAttributesEvent?.Invoke();
    }

    public void OnConsumePotion(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        ConsumePotionEvent?.Invoke();
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        if (context.performed)
            IsAiming = true;
        if (context.canceled)
            IsAiming = false;
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        OnReloadWeapon?.Invoke();
    }
}