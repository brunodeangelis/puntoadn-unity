using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    private Transform _playerTransform;

    private void Awake()
    {
        // Traigo la "cabeza" del jugador (cámara)
        _playerTransform = FindObjectOfType<Player>().transform.GetChild(0).GetChild(0);
    }

    void Update()
    {
        transform.LookAt(_playerTransform, Vector3.up);
    }
}
