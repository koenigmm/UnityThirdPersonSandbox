using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public event Action<Target> OnDestroyed;
    

    private void OnDestroy()
    {
        Debug.Log("Destroy");
        OnDestroyed?.Invoke(this);
    }

    public void DestroyTarget()
    {
        Destroy(this);
    }
}
