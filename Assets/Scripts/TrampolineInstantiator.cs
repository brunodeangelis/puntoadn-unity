using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineInstantiator : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            Instantiate(GameManager._i._trampolinePrefab, transform.GetChild(0).position, Quaternion.identity, transform.parent);
            Debug.Log("instantiated trampoline");
            Destroy(gameObject);
        }
    }
}
