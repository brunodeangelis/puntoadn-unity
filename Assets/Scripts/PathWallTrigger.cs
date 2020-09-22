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
            wall.transform.DOScaleX(0.15f, 0.2f);
            Destroy(gameObject);
        }
    }
}
