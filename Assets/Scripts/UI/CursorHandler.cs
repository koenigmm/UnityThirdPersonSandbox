using System;
using UnityEngine;

public class CursorHandler : MonoBehaviour
{
    private void Start()
    {
        SetCursorActive(true);
    }

    private static void SetCursorActive(bool isActive)
    {
        Cursor.lockState = isActive ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isActive;
    }
}
