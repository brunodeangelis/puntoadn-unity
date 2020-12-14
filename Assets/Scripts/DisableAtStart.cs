using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAtStart : MonoBehaviour {
    private bool _hasAlreadyBeenActivated;

    void Start()
    {
        if (!_hasAlreadyBeenActivated) {
            _hasAlreadyBeenActivated = true;
            gameObject.SetActive(false);
        }
    }
}
