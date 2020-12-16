using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    private bool _canRotate = false;

    [SerializeField] private float _anglesPerSecond = 10f;
    [SerializeField] private bool _xAxis;
    [SerializeField] private bool _yAxis;
    [SerializeField] private bool _zAxis;

    private void Start() {
        Invoke("EnableRotate", Random.Range(0.3f, 2f));
    }

    private void EnableRotate() {
        _canRotate = true;
    }

    private void Update()
    {
        if (!_canRotate) return;
        float rotation = _anglesPerSecond * Time.deltaTime;
        transform.Rotate(_xAxis ? rotation : 0,
                        _yAxis ? rotation : 0,
                        _zAxis ? rotation : 0);
    }
}
