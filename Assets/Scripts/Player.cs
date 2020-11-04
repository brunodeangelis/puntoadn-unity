using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player _instance;

    public static Player Instance { get { return _instance; } }
    public static event Action<Player> OnPlayerDeath;

    private Camera _mainCamera;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        } else
        {
            _instance = this;
        }

        _mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        if (transform.position.y < -45)
        {
            OnPlayerDeath?.Invoke(this);
        }
    }
}
