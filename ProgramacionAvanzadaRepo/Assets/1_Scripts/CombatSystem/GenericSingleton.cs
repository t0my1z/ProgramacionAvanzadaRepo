using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericSingleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;
    public static T Instance { get => _instance; }

    private void Awake()
    {
        _instance = this as T;
    }
}
