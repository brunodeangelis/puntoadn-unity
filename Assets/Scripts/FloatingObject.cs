using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObject : MonoBehaviour {
    [SerializeField] [Range(0f, 5f)] private float _speed = 1f;
    [SerializeField] [Range(0f, 1f)] private float _amplitude = 0.3f;
    private Vector3 _pos;

    private void Start() {
        _pos = transform.position;
    }

    private void Update() {
        Vector3 newPos = new Vector3(_pos.x, _pos.y + _amplitude * Mathf.Sin(Time.time * _speed), _pos.z);
        transform.position = newPos;
    }
}
