using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class IlluminateNearbyPath : MonoBehaviour
{
    [SerializeField] private float _checkRadius = 6f;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, _checkRadius);

            foreach (var hitCollider in hitColliders) {
                WalkingPath path = hitCollider.GetComponent<WalkingPath>();
                if (path == null) continue;
                GameManager._i.IlluminatePath(path);
                break;
            }

            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(transform.position, _checkRadius);
    }
}
