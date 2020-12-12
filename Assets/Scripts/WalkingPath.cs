using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingPath : MonoBehaviour
{
    public PathDirection _pathDirection;
    public Vector3 _scale;
    [Range(0.5f, 10f)] public float _growSpeed = 5f;
}
