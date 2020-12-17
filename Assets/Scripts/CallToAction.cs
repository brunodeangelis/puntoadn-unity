using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallToAction : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        Destroy(gameObject);
    }
}
