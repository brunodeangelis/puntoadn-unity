using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SetVFXRandomColor : MonoBehaviour {
    private VisualEffect _vfx;
    [SerializeField] private string _paramName;

    private void Awake() {
        _vfx = GetComponent<VisualEffect>();
    }

    private void Start() {
        float intensity = 0.03f;
        Vector4 color = new Vector4(Random.Range(0, 255) * intensity, Random.Range(0, 255) * intensity, Random.Range(0, 255) * intensity, 1);
        Debug.Log($"Color: {color}");
        _vfx.SetVector4(_paramName, color);
    }
}
