using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private static Player _instance;

    public static Player Instance { get { return _instance; } }
    public static event Action<Player> OnPlayerDeath;

    [HideInInspector]
    public bool isInsideScreenRange = false;

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this;
        }
    }

    private void LateUpdate() {
        if (transform.position.y < -45) {
            OnPlayerDeath?.Invoke(this);
        }
    }
}
