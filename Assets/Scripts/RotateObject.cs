using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private float _anglesPerSecond = 10f;
    [SerializeField] private bool _xAxis;
    [SerializeField] private bool _yAxis;
    [SerializeField] private bool _zAxis;

    private void Update()
    {
        float rotation = _anglesPerSecond * Time.deltaTime;
        transform.Rotate(_xAxis ? rotation : 0,
                        _yAxis ? rotation : 0,
                        _zAxis ? rotation : 0);
    }
}
