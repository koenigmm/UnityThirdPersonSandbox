using UnityEngine;

public class CursorHandler : MonoBehaviour
{
    [SerializeField] private bool shouldActivateCursorAtStart;
    [SerializeField] private Texture2D cursorTexture;

    private void Start()
    {
        if (shouldActivateCursorAtStart) SetCursorActive(true);
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
    }

    private static void SetCursorActive(bool isActive)
    {
        Cursor.lockState = isActive ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isActive;
    }
}
