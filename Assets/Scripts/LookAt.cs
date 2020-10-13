using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    private Transform _playerTransform;
    private void Awake()
    {
        _playerTransform = FindObjectOfType<Player>().transform;
    }

    void Update() {
        transform.LookAt(_playerTransform, Vector3.up);
    }
}
