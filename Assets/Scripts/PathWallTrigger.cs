using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathWallTrigger : MonoBehaviour
{
    [SerializeField] private GameObject wall;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            wall.transform.DOScale(new Vector3(0.15f, 8f, transform.localScale.z), 0.2f);
            Destroy(gameObject);
        }
    }
}
