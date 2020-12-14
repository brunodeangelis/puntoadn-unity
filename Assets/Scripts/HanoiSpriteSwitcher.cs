using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HanoiSpriteSwitcher : MonoBehaviour {
    [SerializeField] private bool _isLeftArrow;
    private bool _isPlayerLooking;
    private HanoiSymbol _hanoiSymbol;

    private void Start() {
        _hanoiSymbol = transform.parent.GetComponentInChildren<HanoiSymbol>();
    }

    private void Update() {
        if (_isPlayerLooking) {
            if (Input.GetMouseButtonDown(0)) {
                if (_isLeftArrow) {
                    _hanoiSymbol.PreviousSprite();
                } else {
                    _hanoiSymbol.NextSprite();
                }
            }
        }
    }

    private void OnRaycastEnter(GameObject sender) {
        if (sender.CompareTag("Player")) _isPlayerLooking = true;
    }

    private void OnRaycastExit(GameObject sender) {
        if (sender.CompareTag("Player")) _isPlayerLooking = false;
    }
}
