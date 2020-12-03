using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionSetter : MonoBehaviour
{
    [HideInInspector] private PathDirection _direction;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager._i._chosenDirection = _direction;
        }
    }
}
