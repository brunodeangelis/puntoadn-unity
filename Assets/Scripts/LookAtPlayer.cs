using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    private Transform _playerTransform;
    [SerializeField] private bool _onlyYAxis = false;

    private void Awake()
    {
        // Traigo la "cabeza" del jugador (cámara)
        _playerTransform = FindObjectOfType<Player>().transform.GetChild(0).GetChild(0);
    }

    void Update() {
        transform.LookAt(_playerTransform, Vector3.up);
        if (_onlyYAxis) transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }
}
