using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCurrentStation : MonoBehaviour
{
    [SerializeField] private int _stationNumber = 1;

    private void OnTriggerEnter(Collider other) {
        GameManager._i._currentStation = _stationNumber;    
    }
}
