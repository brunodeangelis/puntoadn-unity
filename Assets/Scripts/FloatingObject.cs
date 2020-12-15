using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObject : MonoBehaviour {
    [SerializeField] [Range(0f, 5f)] private float _speed = 1f;
    [SerializeField] [Range(0f, 1f)] private float _amplitude = 0.3f;
    [SerializeField] private bool _xAxis = false;
    [SerializeField] private bool _yAxis = true;
    [SerializeField] private bool _zAxis = false;
    [SerializeField] private bool _inverseDirection = false;
    private Vector3 _pos;

    private void Start() {
        _pos = transform.localPosition;
    }

    private void Update() {
        float _deviation = _amplitude * Mathf.Sin(Time.time * _speed);
        Vector3 newPos;

        if (_inverseDirection) {
            newPos = new Vector3(_xAxis ? _pos.x - _deviation : _pos.x,
                                _yAxis ? _pos.y - _deviation : _pos.y,
                                _zAxis ? _pos.z - _deviation : _pos.z);
        } else {
            newPos = new Vector3(_xAxis ? _pos.x + _deviation : _pos.x,
                                    _yAxis ? _pos.y + _deviation : _pos.y,
                                    _zAxis ? _pos.z + _deviation : _pos.z);
        }
        
        transform.localPosition = newPos;
    }
}
