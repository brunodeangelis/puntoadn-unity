using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetChosenPathColor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            var hitColliders = Physics.OverlapSphere(transform.position, 1.5f);

            foreach (var collider in hitColliders) {
                var path = collider.GetComponent<WalkingPath>();
                if (path == null) continue;

                MaterialPropertyBlock _materialPropertyBlock = new MaterialPropertyBlock();

                var pathRenderer = path.transform.GetChild(0).GetComponent<Renderer>();
                pathRenderer.GetPropertyBlock(_materialPropertyBlock);

                GameManager._i._chosenPathColor = _materialPropertyBlock.GetColor("_EmissionColor");
            }
        }
    }
}
