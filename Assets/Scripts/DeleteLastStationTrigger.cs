using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteLastStationTrigger : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            Destroy(GameManager._i._lastStationSpawned);
            Destroy(gameObject);
        }
    }
}
