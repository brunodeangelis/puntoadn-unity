using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TShirt : MonoBehaviour
{
    [SerializeField] private bool _isCorrectTShirt;
    private bool _isPlayerLooking;

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        _isPlayerLooking = true;
    //        Debug.Log("player entered");
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        _isPlayerLooking = false;
    //        Debug.Log("player left");
    //    }
    //}

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (_isPlayerLooking)
            {
                if (_isCorrectTShirt)
                {
                    TShirt[] tshirts = FindObjectsOfType<TShirt>();
                    foreach (TShirt tshirt in tshirts)
                    {
                        tshirt.transform.DOMoveY(20f, 2f);
                    }
                } else
                {
                    
                }
            }
        }
    }

    void OnRaycastEnter(GameObject sender)
    {
        //GetComponentInChildren<MeshRenderer>().material.color = new Color(136f, 87f, 162f, 1f);
        _isPlayerLooking = true;
        transform.DOScale(transform.localScale + new Vector3(0.1f, 0.1f, 0.1f), 0.3f);
    }

    void OnRaycastExit(GameObject sender)
    {
        //GetComponentInChildren<MeshRenderer>().material.color = Color.white;
        _isPlayerLooking = false;
        transform.DOScale(transform.localScale - new Vector3(0.1f, 0.1f, 0.1f), 0.3f);
    }
}
