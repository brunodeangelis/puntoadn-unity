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

                var led1 = path.transform.GetChild(0);
                var led2 = path.transform.GetChild(1);

                led1.gameObject.SetActive(true);
                led2.gameObject.SetActive(true);

                led1.transform.DOScaleX(1f, 5f);
                led2.transform.DOScaleX(1f, 5f);

                MaterialPropertyBlock _materialPropertyBlock = new MaterialPropertyBlock();

                _materialPropertyBlock.SetColor("_EmissionColor", GameManager._i._chosenPathColor);

                led1.GetComponent<Renderer>().SetPropertyBlock(_materialPropertyBlock);
                led2.GetComponent<Renderer>().SetPropertyBlock(_materialPropertyBlock);

                break;
            }

            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(transform.position, _checkRadius);
    }
}
